using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAiAttackPlayerState : FriendlyAiState
{
    Transform storedPosition;
    public FriendlyAiStateId GetId()
    {
        return FriendlyAiStateId.AttackPlayer;
    }
    public void Enter(FriendlyAiAgent agent)
    {
        if (!agent.noticedPlayer)
        {
            agent.noticedPlayer = true;
        }
        else
        {
            agent.weapon.SetTarget(agent.sensor.GetClosestObject().transform);
        }
        agent.weapon.weaponIK.weight = 1;
    }

    public void Update(FriendlyAiAgent agent)
    {
        if(agent.sensor.PlayerInSight){
            agent.weapon.StartShoot();
            agent.weapon.SetTarget(agent.sensor.GetClosestObject().transform);
            storedPosition = agent.weapon.GetTarget();
        }
        else{
            if(agent.sensor.GetPlayerIsClose()){   
                agent.weapon.StopAllCoroutines();
                agent.weapon.isShooting = false;
                agent.stateMachine.ChangeStates(FriendlyAiStateId.ChasePlayer);
            }
            else{
                agent.weapon.StopAllCoroutines();
                agent.weapon.isShooting = false;
                GameObject temp = new GameObject();
                if (storedPosition != null)
                    temp.transform.position = storedPosition.position;
                else
                    temp.transform.position = agent.transform.position;
                agent.weapon.SetTarget(temp.transform);
                agent.stateMachine.ChangeStates(FriendlyAiStateId.Idle);
            }
        }
    }
    public void Exit(FriendlyAiAgent agent)
    {
        agent.weapon.StopAllCoroutines();
        agent.weapon.SetTarget(null);
    }

}
