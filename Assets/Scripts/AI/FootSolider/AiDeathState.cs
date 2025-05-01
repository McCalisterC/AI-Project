using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;
    public Vector3 grenadePos;
    public float grenadeRadius;
    public bool isGrenadeKill;
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }
    public void Enter(AiAgent agent)
    {
        agent.SpawnAmmoDrop();
        agent.weapon.weaponIK.weight = 0;
        direction.y = 1;
        agent.ragdoll.ActivateRagdoll();
        agent.mesh.updateWhenOffscreen = true;
        agent.DisableEverything();
    }

    public void Update(AiAgent agent)
    {
    }
    public void Exit(AiAgent agent)
    {
    }
    
}
