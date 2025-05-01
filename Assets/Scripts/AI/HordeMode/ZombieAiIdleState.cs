using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAiIdleState : ZombieAiState
{
    public ZombieAiStateId GetId()
    {
        return ZombieAiStateId.Idle;
    }
    public void Enter(ZombieAiAgent agent)
    {
        agent.targetObject = null;
    }
    public void Update(ZombieAiAgent agent)
    {

    }
    public void Exit(ZombieAiAgent agent)
    {
    }
}
