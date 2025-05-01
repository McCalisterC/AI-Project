using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAiAttackPlayerState : SniperAiState
{
    Transform storedPosition;
    public SniperAiStateId GetId()
    {
        return SniperAiStateId.AttackPlayer;
    }
    public void Enter(SniperAiAgent agent)
    {
        agent.weapon.SetTarget(agent.playerObject.GetComponentInChildren<CapsuleCollider>().transform);
        agent.weapon.weaponIK.weight = 1;
    }

    public void Update(SniperAiAgent agent)
    {
        if(agent.sensor.PlayerInSight){
            agent.weapon.StartShoot();
            storedPosition = agent.weapon.GetTarget();
        }
        else{
            agent.weapon.StopAllCoroutines();
            agent.weapon.isShooting = false;
            GameObject temp = new GameObject();
            temp.transform.position = storedPosition.position;
            agent.weapon.SetTarget(temp.transform);
            agent.stateMachine.ChangeStates(SniperAiStateId.Idle);
        }
    }
    public void Exit(SniperAiAgent agent)
    {
        agent.weapon.StopAllCoroutines();
    }

}
