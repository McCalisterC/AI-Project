using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAiChasePlayerState : FriendlyAiState
{
    float timer = 0.0f;
    public FriendlyAiStateId GetId()
    {
        return FriendlyAiStateId.ChasePlayer;
    }
    public void Enter(FriendlyAiAgent agent)
    {
        if(agent.noticedPlayer != true){
            agent.noticedPlayer = true;
        }
        agent.navMeshAgent.isStopped = false;
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
            if(enemy.GetComponentInParent<AIAgentInterface>() != null){
                if(enemy.GetComponentInParent<AIAgentInterface>().HasDied()){
                    agent.stateMachine.ChangeStates(FriendlyAiStateId.FollowPlayer);
                }
            }
            if(!agent.navMeshAgent.hasPath){
                agent.navMeshAgent.destination = enemy.transform.position;
            }

            if(timer < 0.0f){
                Vector3 direction = (enemy.transform.position - agent.navMeshAgent.destination);
                direction.y = 0;
                if(direction.sqrMagnitude > agent.config.maxDistance*agent.config.maxDistance){
                    if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                        agent.navMeshAgent.destination = enemy.transform.position;
                }
                timer = agent.config.maxTime;
            }
        }
        else{
            agent.stateMachine.ChangeStates(FriendlyAiStateId.FollowPlayer);
        }
    }
    public void Exit(FriendlyAiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
    }


}
