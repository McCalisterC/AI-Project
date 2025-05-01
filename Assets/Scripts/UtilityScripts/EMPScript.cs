using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EMPScript : MonoBehaviour, IUtility
{
    [SerializeField] GameObject empPrefab;
    [SerializeField] Camera FPCamera;
    [SerializeField] GameObject utilUI;
    [SerializeField] int cooldownTime;
    [SerializeField] Sprite uiImage;
    [SerializeField] GameObject uiObject;
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
            if(!isOnCooldown){
                isOnCooldown = true;
                OnUtilPress();
            }
        }
    }
    public void OnUtilPress(){
        Ray r = FPCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 dir = r.GetPoint(1) - r.GetPoint(0);
        GameObject tempFrag = Instantiate(empPrefab, this.transform.position, Quaternion.LookRotation(dir));
        tempFrag.GetComponent<Rigidbody>().velocity = tempFrag.transform.forward * 15;
        tempFrag.GetComponent<EMPExplosionScript>().StartExplosion();
        StartCoroutine("Cooldown");
        utilUI.GetComponent<UtilCooldown>().StartCooldownAnim(cooldownTime);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    IEnumerator Cooldown(){
        yield return new WaitForSeconds(cooldownTime);
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        isOnCooldown = false;
    }

    public void DeactivateThis(){
        this.gameObject.SetActive(false);
    }
    public GameObject GetGameObject(){
        return this.gameObject;
    }
}
