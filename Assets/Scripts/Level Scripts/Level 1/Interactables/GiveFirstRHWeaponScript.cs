using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveFirstRHWeaponScript : MonoBehaviour, InteractableInterface
{
    [SerializeField] GameObject grenadeLauncherObject;
    [SerializeField] SetCurrentRHWeapon setCurrentRHWeapon;
    [SerializeField] WeaponManager weaponManager;

    public void Interact(){
        setCurrentRHWeapon.SetRHWeaponButton();
        this.tag = "Untagged";
        weaponManager.ChangeWeapons(grenadeLauncherObject);
    }

    public string Message()
    {
        return "Press F to pick up grenade launcher";
    }
}
