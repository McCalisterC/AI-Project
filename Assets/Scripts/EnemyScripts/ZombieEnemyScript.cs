using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemyScript : MonoBehaviour, IEnemy
{
    private NavMeshAgent agent;
    private Animator thisAnim;
    private Transform target;
    [SerializeField] private float stoppingDistance = 2;
    public int health;
    public int damageAmount;

    private void Update() {
        if(health > 0)
            MoveToTarget();
        else
            agent.isStopped = true;
    }

    private void Awake() {
        GetReferences();
    }

    private void GetReferences(){
        agent = this.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        thisAnim = this.GetComponent<Animator>();
    }

    private void MoveToTarget(){
        agent.SetDestination(target.position);

        RotateToTarget();
        if(agent.velocity != Vector3.zero){
            thisAnim.SetBool("isAttacking", false);
            thisAnim.SetBool("isWalking", true);
        }
        else{
            if(thisAnim.GetBool("isWalking") == true){
                thisAnim.SetBool("isWalking", false);
            }
            Attack();
        }
    }

    private void Attack(){
        thisAnim.SetBool("isAttacking", true);
    }

    private void RotateToTarget(){
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    public void DealDamage(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().StartTakeDamage(50);
    }

    public void TakeDamage(int dealtDamage){
        health -= dealtDamage;

        if(health <= 0){
            Death();
        }
    }

    public void Death(){
        //play animation
        Destroy(this.gameObject);
    }
}
