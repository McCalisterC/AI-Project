using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAiAttackPlayerState : ZombieAiState
{
    Transform storedPosition;
    public ZombieAiStateId GetId()
    {
        return ZombieAiStateId.AttackPlayer;
    }
    public void Enter(ZombieAiAgent agent)
    {
        agent.GetComponent<Animator>().SetBool("isAttacking", true);
    }

    public void Update(ZombieAiAgent agent)
    {
        
    }
    public void Exit(ZombieAiAgent agent)
    {
        agent.GetComponent<Animator>().SetBool("isAttacking", false);
    }

}
