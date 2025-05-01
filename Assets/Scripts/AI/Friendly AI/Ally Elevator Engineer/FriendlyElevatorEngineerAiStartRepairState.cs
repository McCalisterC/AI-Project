using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyElevatorEngineerAiStartRepairState : FriendlyElevatorEngineerAiState
{
    float waitTimer = 2;
    bool _travelling = false;
    bool _isRepairing = false;
    bool _smallWaitDone = false;
    public FriendlyElevatorEngineerAiStateId GetId()
    {
        return FriendlyElevatorEngineerAiStateId.StartRepair;
    }
    public void Enter(FriendlyElevatorEngineerAiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.destination = agent.targets[0].position;
        _travelling = true;
        
    }
    public void Update(FriendlyElevatorEngineerAiAgent agent)
    {
        if(agent.navMeshAgent.destination != null){
            if(_travelling && _smallWaitDone && agent.navMeshAgent.remainingDistance < 1.0f){
                _travelling = false;
                _isRepairing = true;
                StartRepairSequence(agent);
            }
        }

        if(!_smallWaitDone)
            waitTimer -= Time.deltaTime;

        if(waitTimer < 0 && !_smallWaitDone){
            _smallWaitDone = true;
        }
    }
    public void Exit(FriendlyElevatorEngineerAiAgent agent)
    {
        
    }

    public void StartRepairSequence(FriendlyElevatorEngineerAiAgent agent){
        agent.repairPanel.GetComponent<ElevatorRepairSequence>().isRepairing = _isRepairing;
        //Play repairing animation
    }
}