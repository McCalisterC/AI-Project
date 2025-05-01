using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperAiAgent : MonoBehaviour, AIAgentInterface
{
    public SniperAiStateMachine stateMachine;
    public SniperAiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer mesh;
    public GameObject playerObject;
    public AiWeapon weapon;
    public AiSensor sensor;
    public bool hasStartingPath;
    public bool noticedPlayer = false;
    public Transform[] targets;
    public GameObject ammoDrop;
    public GameObject ammoSpawnArea;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<AiWeapon>();
        sensor = GetComponentInChildren<AiSensor>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new SniperAiStateMachine(this);
        stateMachine.RegisterState(new SniperAiDeathState());
        stateMachine.RegisterState(new SniperAiIdleState());
        stateMachine.RegisterState(new SniperAiAttackPlayerState());
        stateMachine.RegisterState(new SniperAiPathfindingState());
        stateMachine.ChangeStates(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        Debug.Log(stateMachine.currentState);
    }

    public void NoticePlayer(){
        noticedPlayer = true;
        weapon.SetTarget(playerObject.transform);
        stateMachine.ChangeStates(SniperAiStateId.AttackPlayer);
    }

    public bool GetNoticePlayer(){
        return noticedPlayer;
    }

    public void Die(Vector3 direction)
    {
        SniperAiDeathState deathState = stateMachine.GetState(SniperAiStateId.Death) as SniperAiDeathState;
        deathState.direction = direction;
        deathState.isGrenadeKill = false;
        ragdoll.ApplyForce(direction * config.dieForce);
        if (stateMachine.currentState != SniperAiStateId.Death)
            stateMachine.ChangeStates(SniperAiStateId.Death);
    }

    public void Die(Vector3 grenadePos, float grenadeRad)
    {
        SniperAiDeathState deathState = stateMachine.GetState(SniperAiStateId.Death) as SniperAiDeathState;
        deathState.direction = Vector3.zero;
        deathState.isGrenadeKill = true;
        deathState.grenadePos = grenadePos;
        deathState.grenadeRadius = grenadeRad;
        ragdoll.ApplyForce(grenadePos, grenadeRad);
        if (stateMachine.currentState != SniperAiStateId.Death)
            stateMachine.ChangeStates(SniperAiStateId.Death);
    }

    public void DisableEverything(){
        weapon.enabled = false;
        sensor.enabled = false;
        navMeshAgent.enabled = false;
    }

    public bool HasDied(){
        if(this.stateMachine.currentState == SniperAiStateId.Death){
            return true;
        }
        else
            return false;
    }

    public void SpawnAmmoDrop()
    {
        //Spawn ammo drop and have it come out of the dead body
        GameObject temp = Instantiate(ammoDrop, ammoSpawnArea.transform.position, Quaternion.identity);
        //Add force to the ammo drop so that it comes out of the dead body in an upwards arc in a random direction
        temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * 100);
    }
}
