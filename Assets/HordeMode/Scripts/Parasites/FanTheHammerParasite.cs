using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTheHammerParasite : MonoBehaviour, IParasite
{
    private int ammoCost;
    private ParasiteManager manager;
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>();
    }
    public void Activate(int level)
    {
        switch (level)
        {
            case 1:
                manager.gun.GetComponent<WeaponScript>().SetHasFanTheHammer(true);
                manager.gun.GetComponent<WeaponScript>().SetFanTheHammerAmmoCost(2);
                break;
            case 2:
                manager.gun.GetComponent<WeaponScript>().SetHasFanTheHammer(true);
                manager.gun.GetComponent<WeaponScript>().SetFanTheHammerAmmoCost(3);
                break;
            case 3:
                manager.gun.GetComponent<WeaponScript>().SetHasFanTheHammer(true);
                manager.gun.GetComponent<WeaponScript>().SetFanTheHammerAmmoCost(6);
                break;
            case 4:
                manager.gun.GetComponent<WeaponScript>().SetHasFanTheHammer(true);
                ammoCost = manager.gun.GetComponent<WeaponScript>().GetAmmoCapacity();
                manager.gun.GetComponent<WeaponScript>().SetFanTheHammerAmmoCost(ammoCost);
                break;
        }   
    }

    public void Deactivate(int level)
    {
        manager.gun.GetComponent<WeaponScript>().SetHasFanTheHammer(false);
        manager.gun.GetComponent<WeaponScript>().SetFanTheHammerAmmoCost(0);
    }
}
