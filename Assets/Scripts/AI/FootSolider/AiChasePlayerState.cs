using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    float timer = 0.0f;
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    public void Enter(AiAgent agent)
    {
        if(agent.noticedPlayer != true){
            agent.noticedPlayer = true;
        }
        agent.navMeshAgent.isStopped = false;
        SetInitialPath(agent);
    }
    public void Update(AiAgent agent)
    {
        GameObject enemy = agent.sensor.GetClosestObject();

        if(enemy != null && agent.sensor.PlayerInSight){
            agent.targetObject = enemy;
            agent.stateMachine.ChangeStates(AiStateId.AttackPlayer);
        }

        if(!agent.enabled){
            return;
        }

        timer -= Time.deltaTime;

        // Check if any AI agents are too close
        Collider[] nearbyAgents = Physics.OverlapSphere(agent.transform.position, agent.minimumDistance, LayerMask.GetMask("Enemy"));
        foreach (var nearbyAgent in nearbyAgents)
        {
            if (nearbyAgent.gameObject != agent.GetComponentInChildren<ThisIsChest>().gameObject)
            {
                Vector3 directionToAgent = nearbyAgent.transform.position - agent.transform.position;
                directionToAgent.y = 0;
                directionToAgent.Normalize();

                // Calculate a position that is minimumDistance away from the nearby agent
                Vector3 targetPosition = nearbyAgent.transform.position + directionToAgent * agent.minimumDistance;

                // Move towards the target position
                agent.navMeshAgent.SetDestination(targetPosition);
            }
        }

        
        if(!agent.navMeshAgent.hasPath){
            agent.navMeshAgent.destination = agent.targetObject.transform.position;
        }

        if(timer < 0.0f){
            Vector3 direction = (agent.targetObject.transform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if(direction.sqrMagnitude > agent.config.maxDistance*agent.config.maxDistance){
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                    agent.navMeshAgent.destination = agent.targetObject.transform.position;
            }
            timer = agent.config.maxTime;
        }

    }
    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
    }

    public void SetInitialPath(AiAgent agent){
        agent.navMeshAgent.destination = agent.targetObject.transform.position;
    }
}
