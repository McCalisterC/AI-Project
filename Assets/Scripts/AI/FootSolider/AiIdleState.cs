using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(AiAgent agent)
    {
        if(agent.hasStartingPath){
            agent.stateMachine.ChangeStates(AiStateId.Pathfind);
        }
    }
    public void Update(AiAgent agent)
    {
        if(agent.sensor.PlayerInSight){
            agent.stateMachine.ChangeStates(AiStateId.AttackPlayer);
        }
    }
    public void Exit(AiAgent agent)
    {
    }
}
