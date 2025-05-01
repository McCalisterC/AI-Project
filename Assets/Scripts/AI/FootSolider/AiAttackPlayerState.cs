using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{
    Transform storedPosition;
    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }
    public void Enter(AiAgent agent)
    {
        if (!agent.noticedPlayer)
        {
            agent.noticedPlayer = true;
        }
        else
        {
            if (agent.sensor.GetClosestObject() != null)
                agent.weapon.SetTarget(agent.sensor.GetClosestObject().transform);
        }
        agent.weapon.weaponIK.weight = 1;
    }

    public void Update(AiAgent agent)
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
                agent.stateMachine.ChangeStates(AiStateId.ChasePlayer);
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
                agent.stateMachine.ChangeStates(AiStateId.Idle);
            }
        }
    }
    public void Exit(AiAgent agent)
    {
        agent.weapon.StopAllCoroutines();
        agent.weapon.SetTarget(null);
    }

}
