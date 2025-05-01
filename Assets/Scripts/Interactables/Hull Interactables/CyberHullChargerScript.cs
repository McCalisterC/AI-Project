using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberHullChargerScript : MonoBehaviour
{
    [SerializeField] StarterAssets.StarterAssetsInputs inputs;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            if(other.GetComponentInChildren<IHullInterface>().name == "CyberHull"){
                StartCoroutine("Charge");
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        StopCoroutine("Charge");
    }

    IEnumerator Charge(){
        inputs.gameObject.GetComponentInChildren<IHullInterface>().GainMeter();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Charge");
    }
}
