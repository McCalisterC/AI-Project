using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    StarterAssets.StarterAssetsInputs inputs;
    [SerializeField]
    UIManagerScript uIManagerScript;
    public void ChangeWeapons(GameObject weapon){
        this.GetComponentInChildren<IWeapons>().Disable();
        GameObject currentWeapon = weapon;
        currentWeapon.SetActive(true);
        inputs.currentWeapon = currentWeapon.GetComponent<IWeapons>();
        uIManagerScript.SetCurrentWeapon(weapon.GetComponent<IWeapons>());
    }

    public StarterAssets.StarterAssetsInputs GetInputs()
    {
        return inputs;
    }
}
