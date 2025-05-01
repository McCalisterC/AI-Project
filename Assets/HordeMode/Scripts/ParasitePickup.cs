using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasitePickup : MonoBehaviour, InteractableInterface
{
    private void Update() {
        Debug.Log("Parasite Pickup");
    }
    public void Interact(){
        GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>().ParasitePickup();
        Destroy(gameObject);
    }

    public string Message()
    {
        return "Press F to consume parasite";
    }
}
