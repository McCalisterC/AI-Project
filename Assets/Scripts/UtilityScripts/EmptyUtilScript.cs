using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EmptyUtilScript : MonoBehaviour, IUtility
{
    [SerializeField] GameObject utilUI;
    StarterAssets.StarterAssetsInputs playerInput;
    bool _isOnCooldown;
    public bool isOnCooldown{get => _isOnCooldown; set => _isOnCooldown = value;}

    private void Awake() {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
        utilUI.SetActive(false);
    }
    private void FixedUpdate() {
        if(playerInput.utility){
            OnUtilPress();
        }
    }
    public void OnUtilPress(){
        //Do nothing
    }

    public void DeactivateThis(){
        utilUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public GameObject GetGameObject(){
        return this.gameObject;
    }
}
