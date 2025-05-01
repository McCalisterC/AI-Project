using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAiChasePlayerState : ZombieAiState
{
    float timer = 0.0f;
    public ZombieAiStateId GetId()
    {
        return ZombieAiStateId.ChasePlayer;
    }
    public void Enter(ZombieAiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        SetInitialPath(agent);
    }
    public void Update(ZombieAiAgent agent)
    {
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

        //When the agent is within the minimum distance, stop moving and change to the attack state
        if (Vector3.Distance(agent.transform.position, agent.targetObject.transform.position) < agent.minimumDistance)
        {
            agent.navMeshAgent.isStopped = true;
            agent.stateMachine.ChangeStates(ZombieAiStateId.AttackPlayer);
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
    public void Exit(ZombieAiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
    }

    public void SetInitialPath(ZombieAiAgent agent){
        agent.navMeshAgent.destination = agent.targetObject.transform.position;
    }
}
