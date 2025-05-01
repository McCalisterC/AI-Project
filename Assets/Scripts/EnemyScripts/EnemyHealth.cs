using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    AIAgentInterface agent;
    SkinnedMeshRenderer skinnedMeshRenderer;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    bool isDead;
    public bool isFriendly = false;

    private void Start() {
        agent = GetComponent<AIAgentInterface>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies){
            if(rigidbody.GetComponent<EnemyHitboxScript>() == null){
                EnemyHitboxScript hitbox = rigidbody.gameObject.AddComponent<EnemyHitboxScript>();
                hitbox.health = this;
                hitbox.isFriendly = isFriendly;
            }
        }
    }
    public void TakeDamage(int dealtDamage, Transform grenadePos, float radius)
    {
        if(!agent.GetNoticePlayer()){
            agent.NoticePlayer();
        }
        currentHealth -= dealtDamage;
        if(currentHealth <= 0.0f){
            Die(grenadePos, radius);
        }

        blinkTimer = blinkDuration;
    }

    public void TakeDamage(int dealtDamage, Vector3 direction, bool isHeadshot)
    {
        if(!agent.GetNoticePlayer()){
            agent.NoticePlayer();
        }
        if(!isDead){
            currentHealth -= dealtDamage;
            if(currentHealth <= 0.0f){
                Die(direction);
            }
        }

        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction){
        isDead = true;
        agent.Die(direction);
    }

    public void Die(Transform grenadePos, float radius){
        isDead = true;
        Vector3 direction = this.transform.position - grenadePos.position;
        agent.Die(grenadePos.position, radius);
    }

    private void Update() {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        if(intensity == 1){
            skinnedMeshRenderer.material.color = Color.white;
        }
        else if(!isDead)
            skinnedMeshRenderer.material.color = Color.red * intensity;
    }
}
