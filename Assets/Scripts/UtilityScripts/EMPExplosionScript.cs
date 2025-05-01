using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPExplosionScript : MonoBehaviour
{
    [SerializeField] GameObject explosionVFX;
    private bool hasHitPlayer;

    public void StartExplosion(){
        StartCoroutine("Explosion");
    }

    IEnumerator Explosion(){
        //Check type damage and disrupt "technical equipment"
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
            }
        }
        else if(other.GetComponent<EnemyHitboxScript>() != null){
            if(other.GetComponent<EnemyHitboxScript>().isHead)
                other.GetComponent<EnemyHitboxScript>().DealDamage(100, this.transform, 10);
        }
    }
}
