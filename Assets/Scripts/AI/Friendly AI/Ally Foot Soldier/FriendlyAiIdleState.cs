using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAiIdleState : FriendlyAiState
{
    float maxTimer = 7.5f;
    float timer = 0.0f;
    public FriendlyAiStateId GetId()
    {
        return FriendlyAiStateId.Idle;
    }
    public void Enter(FriendlyAiAgent agent)
    {
        if(agent.hasStartingPath){
            agent.stateMachine.ChangeStates(FriendlyAiStateId.Pathfind);
        }
        timer = maxTimer;
    }
    public void Update(FriendlyAiAgent agent)
    {
        timer -= Time.deltaTime;
        if(timer <= 0){
            agent.stateMachine.ChangeStates(FriendlyAiStateId.FollowPlayer);
        }

        if(agent.sensor.PlayerInSight){
            agent.stateMachine.ChangeStates(FriendlyAiStateId.AttackPlayer);
        }
    }
    public void Exit(FriendlyAiAgent agent)
    {
    }
}
