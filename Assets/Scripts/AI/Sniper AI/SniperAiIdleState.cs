using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAiIdleState : SniperAiState
{
    public SniperAiStateId GetId()
    {
        return SniperAiStateId.Idle;
    }
    public void Enter(SniperAiAgent agent)
    {
        if(agent.hasStartingPath){
            agent.stateMachine.ChangeStates(SniperAiStateId.Pathfind);
        }
    }
    public void Update(SniperAiAgent agent)
    {
        if(agent.sensor.PlayerInSight){
            agent.stateMachine.ChangeStates(SniperAiStateId.AttackPlayer);
        }
    }
    public void Exit(SniperAiAgent agent)
    {
    }
}
