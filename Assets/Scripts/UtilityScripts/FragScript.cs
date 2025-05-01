using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FragScript : MonoBehaviour, IUtility
{
    [SerializeField] GameObject fragPrefab;
    [SerializeField] Camera FPCamera;
    [SerializeField] GameObject utilUI;
    [SerializeField] int cooldownTime;
    [SerializeField] Sprite uiImage;
    [SerializeField] GameObject uiObject;
    [SerializeField] float boostForce = 10f;
    StarterAssets.StarterAssetsInputs playerInput;
    bool _isOnCooldown;
    public bool isOnCooldown{get => _isOnCooldown; set => _isOnCooldown = value;}

    private void Awake() {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
        if(uiObject.GetComponent<Image>().sprite != uiImage){
            uiObject.GetComponent<Image>().sprite = uiImage;
        }
    }
    private void FixedUpdate() {
        if(playerInput.utility){
            if(!_isOnCooldown){
                _isOnCooldown = true;
                OnUtilPress();
            }
        }
    }
    public void OnUtilPress(){
        Ray r = FPCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 dir = r.GetPoint(1) - r.GetPoint(0);
        GameObject tempFrag = Instantiate(fragPrefab, this.transform.position, Quaternion.LookRotation(dir));
        tempFrag.GetComponent<Rigidbody>().velocity = tempFrag.transform.forward * 15;
        tempFrag.GetComponent<FragExplodeScript>().StartExplosion(boostForce);
        StartCoroutine("Cooldown");
        utilUI.GetComponent<UtilCooldown>().StartCooldownAnim(cooldownTime);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    IEnumerator Cooldown(){
        yield return new WaitForSeconds(cooldownTime);
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        _isOnCooldown = false;
    }

    public void DeactivateThis(){
        this.gameObject.SetActive(false);
    }
    public GameObject GetGameObject(){
        return this.gameObject;
    }
}
