using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public int damage = 50;
    private ZombieAiAgent agent;

    private void Awake() {
        agent = this.GetComponent<ZombieAiAgent>();
    }

    // Check if the zombie is still in range of the player, and if so, deal damage
    private void CheckIfAttackHits() {
        if(Vector3.Distance(agent.transform.position, agent.targetObject.transform.position) < agent.minimumDistance){
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().StartTakeDamage(damage);
        }
    }

    // Check if the zombie is still in range of the player, and if not, change to the chase state
    private void CheckIfAttackMisses() {
        if(Vector3.Distance(agent.transform.position, agent.targetObject.transform.position) > agent.minimumDistance){
            agent.stateMachine.ChangeStates(ZombieAiStateId.ChasePlayer);
        }
    }
}
