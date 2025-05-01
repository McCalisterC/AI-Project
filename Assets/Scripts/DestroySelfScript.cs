using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfScript : MonoBehaviour
{
    [SerializeField] float timeTilDeath;

    private void Awake() {
        StartCoroutine("Die");
    }

    IEnumerator Die(){
        yield return new WaitForSeconds(timeTilDeath);
        Destroy(this.gameObject);
    }
}
