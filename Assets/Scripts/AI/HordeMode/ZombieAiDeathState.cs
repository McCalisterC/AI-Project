using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAiDeathState : ZombieAiState
{
    public Vector3 direction;
    public Vector3 grenadePos;
    public float grenadeRadius;
    public bool isGrenadeKill;
    public ZombieAiStateId GetId()
    {
        return ZombieAiStateId.Death;
    }
    public void Enter(ZombieAiAgent agent)
    {
        if(agent.isInfectedWithParasite){
            agent.SpawnParasitePickup();
        }
        agent.OnKillModification();
        direction.y = 1;
        agent.ragdoll.ActivateRagdoll();
        agent.mesh.updateWhenOffscreen = true;
        agent.DisableEverything();
    }

    public void Update(ZombieAiAgent agent)
    {
    }
    public void Exit(ZombieAiAgent agent)
    {
    }
    
}
