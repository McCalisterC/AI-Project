using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxScript : MonoBehaviour
{
    public EnemyHealth health;
    public bool isHead = false;
    public bool isFriendly = false;

    public void DealDamage(int damage, Vector3 direction, bool isHeadshot){
        health.TakeDamage(damage, direction, isHeadshot);
    }

    public void DealDamage(int damage, Transform grenadePos, float grenadeRad){
        health.TakeDamage(damage, grenadePos, grenadeRad);
    }
}
