using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class KnifeScript : MonoBehaviour
{
    [SerializeField] GameObject weaponParent;
    [SerializeField] RadialMenu radialMenu;
    [SerializeField] PlayerInteractionScript playerInteractionScript;
    [SerializeField] Material emissionMat;
    private PlayerStats playerStats;
    private Color emissionColor;
    private GameObject currentWeapon;
    private GameObject currentUtil;
    private StarterAssets.StarterAssetsInputs inputs;
    private Animator thisAnim;
    private bool isKnifing;

    private void Awake() {
        thisAnim = this.GetComponent<Animator>();
        inputs = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        SetEmissiveColor();
    }

    public void SetEmissiveColor(){
        emissionColor = playerStats.GetCurrentHull().GetColor();
        HDMaterial.SetEmissiveColor(emissionMat, emissionColor);
    }

    private void FixedUpdate() {
        if(inputs.knife && !isKnifing){
            isKnifing = true;
            Knife();
        }
    }

    public void Knife(){
        thisAnim.SetTrigger("Knife");
    }

    private void GetCurrentWeaponAndUtil(){
        currentWeapon = weaponParent.GetComponentInChildren<IWeapons>().GetGameObject();
        currentUtil = weaponParent.GetComponentInChildren<IUtility>().GetGameObject();
    }

    public void StartKnife(){
        GetCurrentWeaponAndUtil();
        currentWeapon.SetActive(false);
        currentUtil.SetActive(false);
        radialMenu.canOpen = false;
        playerInteractionScript.canInteract = false;
    }

    public void EndKnife(){
        currentWeapon.SetActive(true);
        currentUtil.SetActive(true);
        radialMenu.canOpen = true;
        playerInteractionScript.canInteract = true;
        isKnifing = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<EnemyHitboxScript>() != null){
            other.GetComponent<EnemyHitboxScript>().DealDamage(100, this.transform.forward, false);
        }
    }
}
