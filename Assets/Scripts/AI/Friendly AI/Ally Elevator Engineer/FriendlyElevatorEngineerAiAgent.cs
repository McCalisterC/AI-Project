using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyElevatorEngineerAiAgent : MonoBehaviour, AIAgentInterface
{
    public FriendlyElevatorEngineerAiStateMachine stateMachine;
    public FriendlyElevatorEngineerAiStateId initialState;
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
    public GameObject repairPanel;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<AiWeapon>();
        sensor = GetComponentInChildren<AiSensor>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new FriendlyElevatorEngineerAiStateMachine(this);
        stateMachine.RegisterState(new FriendlyElevatorEngineerAiIdleState());
        stateMachine.RegisterState(new FriendlyElevatorEngineerAiStartRepairState());
        stateMachine.RegisterState(new FriendlyElevatorEngineerAiEndRepairState());
        stateMachine.ChangeStates(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        Debug.Log(stateMachine.currentState);
    }

    public void NoticePlayer(){
        //Engineer does not attack
    }

    public bool GetNoticePlayer(){
        return noticedPlayer;
    }

    public void Die(Vector3 direction){
        //Engineer cannot die
    }

    public void Die(Vector3 grenadePos, float grenadeRad){
        //Engineer cannot die
    }

    public void DisableEverything(){
        weapon.enabled = false;
        sensor.enabled = false;
        navMeshAgent.enabled = false;
    }

    public bool HasDied(){
        //Engineer cannot die
        return false;
    }
}
