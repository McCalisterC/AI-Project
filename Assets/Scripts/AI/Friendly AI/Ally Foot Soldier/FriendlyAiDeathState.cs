using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAiDeathState : FriendlyAiState
{
    public Vector3 direction;
    public Vector3 grenadePos;
    public float grenadeRadius;
    public bool isGrenadeKill;
    public FriendlyAiStateId GetId()
    {
        return FriendlyAiStateId.Death;
    }
    public void Enter(FriendlyAiAgent agent)
    {
        agent.weapon.weaponIK.weight = 0;
        direction.y = 1;
        agent.ragdoll.ActivateRagdoll();
        if(isGrenadeKill){
            agent.ragdoll.ApplyForce(grenadePos, grenadeRadius);
        }
        else
            agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.mesh.updateWhenOffscreen = true;
        agent.DisableEverything();
    }

    public void Update(FriendlyAiAgent agent)
    {
    }
    public void Exit(FriendlyAiAgent agent)
    {
    }
    
}
