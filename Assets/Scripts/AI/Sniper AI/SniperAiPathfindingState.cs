using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAiPathfindingState : SniperAiState
{
    int count = 0;
    float waitTimer = 2;
    bool _travelling = false;
    bool _waiting = false;
    public SniperAiStateId GetId()
    {
        return SniperAiStateId.Pathfind;
    }
    public void Enter(SniperAiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.speed = agent.navMeshAgent.speed / 2;
        agent.navMeshAgent.destination = agent.targets[0].position;
        count++;
        _travelling = true;
    }
    public void Update(SniperAiAgent agent)
    {
        Debug.Log(agent.navMeshAgent.destination);
        if(_travelling && agent.navMeshAgent.remainingDistance < 1.0f){
            _travelling = false;
            _waiting = true;
        }

        if(_waiting){
            waitTimer -= Time.deltaTime;
            if(waitTimer <= 0){
                _waiting = false;
                GoToNextTarget(agent);
            }
        }

        if(agent.sensor.IsInSight(agent.playerObject)){
            agent.stateMachine.ChangeStates(SniperAiStateId.AttackPlayer);
        }
    }
    public void Exit(SniperAiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.speed = agent.navMeshAgent.speed * 2;
    }

    public void GoToNextTarget(SniperAiAgent agent){
        GetNextTarget(agent);
        waitTimer = 2;
        _travelling = true;
    }

    public void GetNextTarget(SniperAiAgent agent){
        if(count == agent.targets.Length){
            count = 0;
            agent.navMeshAgent.destination = agent.targets[count].position;
        }
        else{
            agent.navMeshAgent.destination = agent.targets[count].position;
            count++;
        }
    }
}
