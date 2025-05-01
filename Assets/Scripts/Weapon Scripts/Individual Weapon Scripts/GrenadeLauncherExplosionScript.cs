using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherExplosionScript : MonoBehaviour
{
    [SerializeField]public GameObject explosionVFX;
    private bool hasHitPlayer;

    private void Awake() {
        StartCoroutine(CountdownUntilDisappear());
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "PlayerCollider")
            StartCoroutine(Explosion());
    }

    IEnumerator Explosion(){
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
        Instantiate(explosionVFX, this.transform.position, Quaternion.identity);
        yield return new WaitForEndOfFrame();
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

    IEnumerator CountdownUntilDisappear(){
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
