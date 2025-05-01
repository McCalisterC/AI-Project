using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AiWeapon : MonoBehaviour
{
    [SerializeField] Transform rayPosition;
    [Header("Basic Stats")]
    [SerializeField] float range = 100f;
    [SerializeField] int damageAmount;
    [SerializeField] double fireRate;
    [Tooltip("How Many Bullets Can Be Shot Per Second")]
    [SerializeField] float inaccuracyDistance = 5f;
    private int chanceToHit;
    private Animator thisAnim;
    PlayerStats playerStats;
    public WeaponIK weaponIK;
    public bool isShooting;

    public void Awake(){
        thisAnim = this.gameObject.GetComponent<Animator>();
        weaponIK = GetComponent<WeaponIK>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if(GameObject.FindGameObjectWithTag("Settings") != null)
        {
            SetChanceToHit(GameObject.FindGameObjectWithTag("Settings").GetComponent<SettingsScript>().GetDifficulty());
        }
        else
        {
            SetChanceToHit(1);
        }
    }

    public void StartShoot(){
        if(!isShooting){
            isShooting = true;
            StartCoroutine(CanNotShoot());
        }
    }
    public void Shoot(){
        RaycastHit hit;
        Vector3 direction = GetShootingDirection();
        Physics.Raycast(rayPosition.position, direction, out hit, range);
        StartCoroutine("CanNotShoot");
        if(hit.collider.tag == "PlayerCollider"){
            if(Random.Range(0,100) <= chanceToHit)
                playerStats.StartTakeDamage(damageAmount);
        }
        else if(hit.collider.GetComponent<EnemyHitboxScript>() != null){
            if (hit.collider.GetComponent<EnemyHitboxScript>().isFriendly != GetComponent<EnemyHealth>().isFriendly)
                hit.collider.GetComponent<EnemyHitboxScript>().DealDamage(damageAmount, direction, false);
        }
        thisAnim.SetTrigger("Shoot");
    }

    IEnumerator CanNotShoot(){
        yield return new WaitForSeconds((float)(1.0/fireRate));
        Shoot();
    }

    Vector3 GetShootingDirection(){
        Vector3 targetPos = rayPosition.position + rayPosition.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(inaccuracyDistance, inaccuracyDistance),
            targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance)
        );

        Vector3 direction = targetPos - rayPosition.position;
        return direction.normalized;
    }

    public void SetTarget(Transform target){
        weaponIK.SetTargetTranform(target);
    }

    public Transform GetTarget(){
        return weaponIK.targetTransform;
    }

    private void SetChanceToHit(int difficulty){
        switch(difficulty){
            case 0:
                chanceToHit = 25;
                break;
            case 1:
                chanceToHit = 50;
                break;
            case 2:
                chanceToHit = 75;
                break;
            case 3:
                chanceToHit = 100;
                break;
            default:
                chanceToHit = 50;
                break;
        }
    }
}

