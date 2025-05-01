using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussianRouletteParasite : MonoBehaviour, IParasite
{
    private ParasiteManager manager;
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>();
    }
    public void Activate(int level){
        switch (level)
        {
            case 1:
                manager.reloadSpeedModifier += 0.1f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = true;
                break;
            case 2:
                manager.reloadSpeedModifier += 0.2f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = true;
                break;
            case 3:
                manager.reloadSpeedModifier += 0.3f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = true;
                break;
            case 4:
                manager.reloadSpeedModifier += 0.5f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = true;
                break;
        }
    }

    public void Deactivate(int level)
    {
        switch (level)
        {
            case 1:
                manager.reloadSpeedModifier -= 0.1f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = false;
                break;
            case 2:
                manager.reloadSpeedModifier -= 0.2f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = false;
                break;
            case 3:
                manager.reloadSpeedModifier -= 0.3f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = false;
                break;
            case 4:
                manager.reloadSpeedModifier -= 0.5f;
                manager.ModifyReloadSpeed();
                manager.russianRouletteModifier = false;
                break;
        }
    }
}
