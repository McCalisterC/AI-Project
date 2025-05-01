using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedDiceParasite : MonoBehaviour, IParasite
{
    private ParasiteManager manager;
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>();
    }
    public void Activate(int level){
        switch (level)
        {
            case 1:
                manager.loadedDiceModifier = 25;
                manager.critDamageModifier -= 0.50f;
                manager.ModifyCritDamage();
                break;
            case 2:
                manager.loadedDiceModifier = 50;
                manager.critDamageModifier -= 0.75f;
                manager.ModifyCritDamage();
                break;
            case 3:
                manager.loadedDiceModifier = 75;
                manager.critDamageModifier -= 1.00f;
                manager.ModifyCritDamage();
                break;
            case 4:
                manager.loadedDiceModifier = 100;
                manager.critDamageModifier -= 1.25f;
                manager.ModifyCritDamage();
                break;
        }
    }

    public void Deactivate(int level)
    {
        switch (level)
        {
            case 1:
                manager.loadedDiceModifier = 0;
                manager.critDamageModifier += 0.50f;
                manager.ModifyCritDamage();
                break;
            case 2:
                manager.loadedDiceModifier = 0;
                manager.critDamageModifier += 0.75f;
                manager.ModifyCritDamage();
                break;
            case 3:
                manager.loadedDiceModifier = 0;
                manager.critDamageModifier += 1.00f;
                manager.ModifyCritDamage();
                break;
            case 4:
                manager.loadedDiceModifier = 0;
                manager.critDamageModifier += 1.25f;
                manager.ModifyCritDamage();
                break;
        }
    }
}
