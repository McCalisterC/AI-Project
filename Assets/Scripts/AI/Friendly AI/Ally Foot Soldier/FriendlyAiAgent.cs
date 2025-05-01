using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAiAgent : MonoBehaviour, AIAgentInterface
{
    public FriendlyAiStateMachine stateMachine;
    public FriendlyAiStateId initialState;
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

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<AiWeapon>();
        sensor = GetComponentInChildren<AiSensor>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new FriendlyAiStateMachine(this);
        stateMachine.RegisterState(new FriendlyAiChasePlayerState());
        stateMachine.RegisterState(new FriendlyAiDeathState());
        stateMachine.RegisterState(new FriendlyAiIdleState());
        stateMachine.RegisterState(new FriendlyAiAttackPlayerState());
        stateMachine.RegisterState(new FriendlyAiPathfindingState());
        stateMachine.RegisterState(new FriendlyAiFollowState());
        stateMachine.ChangeStates(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        Debug.Log(stateMachine.currentState);
    }

    public void NoticePlayer(){
        weapon.SetTarget(playerObject.transform);
        stateMachine.ChangeStates(FriendlyAiStateId.AttackPlayer);
    }

    public bool GetNoticePlayer(){
        return noticedPlayer;
    }

    public void Die(Vector3 direction){
        FriendlyAiDeathState deathState = stateMachine.GetState(FriendlyAiStateId.Death) as FriendlyAiDeathState;
        deathState.direction = direction;
        deathState.isGrenadeKill = false;
        stateMachine.ChangeStates(FriendlyAiStateId.Death);
    }

    public void Die(Vector3 grenadePos, float grenadeRad){
        FriendlyAiDeathState deathState = stateMachine.GetState(FriendlyAiStateId.Death) as FriendlyAiDeathState;
        deathState.direction = Vector3.zero;
        deathState.isGrenadeKill = true;
        deathState.grenadePos = grenadePos;
        deathState.grenadeRadius = grenadeRad;
        stateMachine.ChangeStates(FriendlyAiStateId.Death);
    }

    public void DisableEverything(){
        weapon.enabled = false;
        sensor.enabled = false;
        navMeshAgent.enabled = false;
    }

    public bool HasDied(){
        if(this.stateMachine.currentState == FriendlyAiStateId.Death){
            return true;
        }
        else
            return false;
    }
}
