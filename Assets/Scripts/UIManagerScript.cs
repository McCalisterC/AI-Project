using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TMP_Text currentAmmoUI;
    [SerializeField] TMP_Text maxAmmoCapacityUI;
    [SerializeField] GameObject interactionText;

    [Header("Required Fields")]
    [SerializeField] GameObject weaponManager;
    private IWeapons currentWeapon;

    private void Awake() {
        currentWeapon = weaponManager.GetComponentInChildren<IWeapons>();
    }

    private void Start()
    {
        SetWeaponUI();
    }

    public void SetWeaponUI(){
        SetWeaponCurrentAmmo();
        if(currentWeapon.GetAmmoCapacity() == -1)
            maxAmmoCapacityUI.text = "/ ∞";
        else
            maxAmmoCapacityUI.text = "/ " + currentWeapon.GetAmmoCapacity().ToString();
    }

    public void SetWeaponCurrentAmmo(){
        currentAmmoUI.text = currentWeapon.currentAmmoAmount.ToString();
    }

    public void ActivateInteractionText(){
        if(!interactionText.activeSelf)
            interactionText.SetActive(true);
    }

    public void DeactivateInteractionText(){
        if(interactionText.activeSelf)
            interactionText.SetActive(false);
    }

    public void SetCurrentWeapon(IWeapons weapon){
        currentWeapon = weapon;
        SetWeaponUI();
    }

    public void SetInteractionText(string text)
    {
        interactionText.GetComponent<TMP_Text>().text = text;
    }
}
