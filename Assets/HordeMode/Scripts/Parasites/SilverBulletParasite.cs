using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverBulletParasite : MonoBehaviour, IParasite
{
    //Silver bullet gives the player a higher chance to get a critical hit, but decreases the fire rate of the weapon

    private ParasiteManager manager;
    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>();
    }
    public void Activate(int level)
    {
        switch (level)
        {
            case 1:
                manager.critDamageModifier += 0.25f;
                manager.fireRateModifier -= 0.05f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
            case 2:
                manager.critDamageModifier += 0.50f;
                manager.fireRateModifier -= 0.10f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
            case 3:
                manager.critDamageModifier += 0.75f;
                manager.fireRateModifier -= 0.15f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
            case 4:
                manager.critDamageModifier += 2.00f;
                manager.fireRateModifier -= 0.50f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
        }
    }

    public void Deactivate(int level)
    {
        switch (level)
        {
            case 1:
                manager.critDamageModifier -= 0.25f;
                manager.fireRateModifier += 0.05f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
            case 2:
                manager.critDamageModifier -= 0.50f;
                manager.fireRateModifier += 0.10f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
            case 3:
                manager.critDamageModifier -= 0.75f;
                manager.fireRateModifier += 0.15f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
            case 4:
                manager.critDamageModifier -= 2.00f;
                manager.fireRateModifier += 0.50f;
                manager.ModifyCritDamage();
                manager.ModifyFireRate();
                break;
        }
    }
}
