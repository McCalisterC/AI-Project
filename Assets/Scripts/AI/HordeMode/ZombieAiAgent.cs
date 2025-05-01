using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAiAgent : MonoBehaviour, AIAgentInterface
{
    public ZombieAiStateMachine stateMachine;
    public ZombieAiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer mesh;
    public GameObject targetObject;
    public float minimumDistance = 1.0f;
    public bool isInfectedWithParasite = false;
    public GameObject parasiteSpawnArea;
    public GameObject[] parasitePrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(targetObject == null)
            targetObject = GameObject.FindGameObjectWithTag("Player");
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(stateMachine == null)
            MakeStateMachine();
        stateMachine.RegisterState(new ZombieAiChasePlayerState());
        stateMachine.RegisterState(new ZombieAiDeathState());
        stateMachine.RegisterState(new ZombieAiIdleState());
        stateMachine.RegisterState(new ZombieAiAttackPlayerState());
        stateMachine.ChangeStates(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public void NoticePlayer(){
        //Do Nothing, needed for Interface
    }

    public bool GetNoticePlayer(){
        //Do Nothing, needed for Interface
        return false;
    }

    public void Die(Vector3 direction){
        ZombieAiDeathState deathState = stateMachine.GetState(ZombieAiStateId.Death) as ZombieAiDeathState;
        deathState.direction = direction;
        deathState.isGrenadeKill = false;
        ragdoll.ApplyForce(direction * config.dieForce);
        GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>().ZombiesKilled++;
        GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>().ZombiesThisRound--;
        if (stateMachine.currentState != ZombieAiStateId.Death)
            stateMachine.ChangeStates(ZombieAiStateId.Death);
    }

    public void Die(Vector3 grenadePos, float grenadeRad){
        ZombieAiDeathState deathState = stateMachine.GetState(ZombieAiStateId.Death) as ZombieAiDeathState;
        deathState.direction = Vector3.zero;
        deathState.isGrenadeKill = true;
        deathState.grenadePos = grenadePos;
        deathState.grenadeRadius = grenadeRad;
        ragdoll.ApplyForce(grenadePos, grenadeRad);    
        if (stateMachine.currentState != ZombieAiStateId.Death)
            stateMachine.ChangeStates(ZombieAiStateId.Death);
    }

    public void DisableEverything(){
        navMeshAgent.enabled = false;
    }

    public bool HasDied(){
        if(this.stateMachine.currentState == ZombieAiStateId.Death){
            return true;
        }
        else
            return false;
    }

    public void MakeStateMachine(){
        stateMachine = new ZombieAiStateMachine(this);
    }

    public void SpawnParasitePickup(){
        int tempNum = Random.Range(0, parasitePrefab.Length);
        // Spawn ammo drop and have it come out of the dead body
        GameObject temp = Instantiate(parasitePrefab[tempNum], parasiteSpawnArea.transform.position, Quaternion.identity);
        //Add force to the ammo drop so that it comes out of the dead body in an upwards arc in a random direction
        temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * 100);
    }

    public void OnKillModification(){
        if(GameObject.FindGameObjectWithTag("ParasiteManager").GetComponentInChildren<SwiftSpursParasite>().IsActivated){
            GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>().StartOnKillMovementModification();
        }
    }
}
