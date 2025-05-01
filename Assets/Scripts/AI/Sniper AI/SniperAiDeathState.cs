using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAiDeathState : SniperAiState
{
    public Vector3 direction;
    public Vector3 grenadePos;
    public float grenadeRadius;
    public bool isGrenadeKill;
    public SniperAiStateId GetId()
    {
        return SniperAiStateId.Death;
    }
    public void Enter(SniperAiAgent agent)
    {
        agent.SpawnAmmoDrop();
        agent.weapon.weaponIK.weight = 0;
        direction.y = 1;
        agent.ragdoll.ActivateRagdoll();
        agent.mesh.updateWhenOffscreen = true;
        agent.DisableEverything();
    }

    public void Update(SniperAiAgent agent)
    {
    }
    public void Exit(SniperAiAgent agent)
    {
    }
    
}
