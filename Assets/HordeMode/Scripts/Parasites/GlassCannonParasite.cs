using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCannonParasite : MonoBehaviour, IParasite
{
    private ParasiteManager manager;
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>();
    }
    public void Activate(int level)
    {
        switch (level)
        {
            case 1:
                manager.maxHPModifier -= 0.25f;
                manager.damageModifier += 0.5f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
            case 2:
                manager.maxHPModifier -= 0.5f;
                manager.damageModifier += 1f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
            case 3:
                manager.maxHPModifier -= 0.75f;
                manager.damageModifier += 2f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
            case 4:
                manager.maxHPModifier -= 2f;
                manager.damageModifier += 3f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
        }
    }

    public void Deactivate(int level)
    {
        switch (level)
        {
            case 1:
                manager.maxHPModifier += 0.25f;
                manager.damageModifier -= 0.5f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
            case 2:
                manager.maxHPModifier += 0.5f;
                manager.damageModifier -= 1f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
            case 3:
                manager.maxHPModifier += 0.75f;
                manager.damageModifier -= 2f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
            case 4:
                manager.maxHPModifier += 2f;
                manager.damageModifier -= 3f;
                manager.ModifyHP();
                manager.ModifyDamage();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SetHealthUI();
                break;
        }
    }
}
