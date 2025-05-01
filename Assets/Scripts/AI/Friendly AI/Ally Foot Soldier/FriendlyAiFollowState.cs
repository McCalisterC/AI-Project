using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAiFollowState : FriendlyAiState
{
    float timer = 0.0f;
    public FriendlyAiStateId GetId()
    {
        return FriendlyAiStateId.FollowPlayer;
    }
    public void Enter(FriendlyAiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.destination = agent.playerObject.transform.position;
    }
    public void Update(FriendlyAiAgent agent)
    {
        if(agent.sensor.PlayerInSight){
            agent.stateMachine.ChangeStates(FriendlyAiStateId.AttackPlayer);
        }

        if(!agent.enabled){
            return;
        }

        timer -= Time.deltaTime;
        GameObject enemy = agent.sensor.GetClosestObject();
        if(enemy != null){  
            if(!agent.navMeshAgent.hasPath){
                agent.navMeshAgent.destination = enemy.transform.position;
            }
        }
        else{
            if(!agent.navMeshAgent.hasPath){
                agent.navMeshAgent.destination = agent.playerObject.transform.position;
            }
        }

        if(timer < 0.0f){
            Vector3 direction;
            if(enemy != null){
                direction = (enemy.transform.position - agent.navMeshAgent.destination);
                direction.y = 0;
                if(direction.sqrMagnitude > agent.config.maxDistance*agent.config.maxDistance){
                    if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                        agent.navMeshAgent.destination = enemy.transform.position;
                }
            }
            else{
                direction = (agent.playerObject.transform.position - agent.navMeshAgent.destination);
                direction.y = 0;
                if(direction.sqrMagnitude > agent.config.maxDistance*agent.config.maxDistance){
                    if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                        agent.navMeshAgent.destination = agent.playerObject.transform.position;
                }
            }
            timer = agent.config.maxTime;
        }
    }
    public void Exit(FriendlyAiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
    }
}
