using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragExplodeScript : MonoBehaviour
{
    [SerializeField] GameObject explosionVFX;
    private float _boostForce;
    private bool hasHitPlayer;

    public void StartExplosion(float boostForce){
        StartCoroutine("Explosion");
        _boostForce = boostForce;
    }

    IEnumerator Explosion(){
        //Insert Particle System Detination and Damage
        yield return new WaitForSeconds(2.6f);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
        Instantiate(explosionVFX, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            if(!hasHitPlayer){
                other.GetComponent<PlayerStats>().StartTakeDamage(70);
                hasHitPlayer = true;
                // Apply an additional force to the player's rigidbody or controller during the boost
                other.GetComponent<CharacterController>().Move(transform.forward * _boostForce * Time.fixedDeltaTime);
            }
        }
        else if(other.GetComponent<EnemyHitboxScript>() != null){
            if(other.GetComponent<EnemyHitboxScript>().isHead)
                other.GetComponent<EnemyHitboxScript>().DealDamage(100, this.transform, 10);
        }
    }
}
