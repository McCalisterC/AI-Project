using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour, AIAgentInterface
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer mesh;
    public GameObject targetObject;
    public AiWeapon weapon;
    public AiSensor sensor;
    public bool hasStartingPath;
    public bool noticedPlayer = false;
    public Transform[] targets;
    public float minimumDistance = 1.0f;
    public GameObject ammoDrop;
    public GameObject ammoSpawnArea;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<AiWeapon>();
        sensor = GetComponentInChildren<AiSensor>();
        if(targetObject == null)
            targetObject = GameObject.FindGameObjectWithTag("Player");
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(stateMachine == null)
            MakeStateMachine();
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiAttackPlayerState());
        stateMachine.RegisterState(new AiPathfindingState());
        stateMachine.ChangeStates(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        Debug.Log(stateMachine.currentState);
    }

    public void NoticePlayer(){
        weapon.SetTarget(targetObject.transform);
        stateMachine.ChangeStates(AiStateId.AttackPlayer);
    }

    public bool GetNoticePlayer(){
        return noticedPlayer;
    }

    public void Die(Vector3 direction){
        AiDeathState deathState = stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        deathState.isGrenadeKill = false;
        ragdoll.ApplyForce(direction * config.dieForce);
        if (stateMachine.currentState != AiStateId.Death)
            stateMachine.ChangeStates(AiStateId.Death);
    }

    public void Die(Vector3 grenadePos, float grenadeRad){
        AiDeathState deathState = stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = Vector3.zero;
        deathState.isGrenadeKill = true;
        deathState.grenadePos = grenadePos;
        deathState.grenadeRadius = grenadeRad;
        ragdoll.ApplyForce(grenadePos, grenadeRad);
        if (stateMachine.currentState != AiStateId.Death)
            stateMachine.ChangeStates(AiStateId.Death);
    }

    public void DisableEverything(){
        weapon.enabled = false;
        sensor.enabled = false;
        navMeshAgent.enabled = false;
    }

    public bool HasDied(){
        if(this.stateMachine.currentState == AiStateId.Death){
            return true;
        }
        else
            return false;
    }

    public void MakeStateMachine(){
        stateMachine = new AiStateMachine(this);
    }

    public void SpawnAmmoDrop()
    {
        //Spawn ammo drop and have it come out of the dead body
        GameObject temp = Instantiate(ammoDrop, ammoSpawnArea.transform.position, Quaternion.identity);
        //Add force to the ammo drop so that it comes out of the dead body in an upwards arc in a random direction
        temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * 100);
    }
}
