using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentRHWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] WeaponWheelScriptableObject weaponLabel;
    [SerializeField] RadialMenu radialMenu;

    public void SetRHWeaponButton(){
        radialMenu.SetRHWeapon(weapon, weaponLabel);
    }
}
