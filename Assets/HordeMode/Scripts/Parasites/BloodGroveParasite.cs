using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGroveParasite : MonoBehaviour, IParasite
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
                manager.regenCounterModifier += 2;
                manager.lifeStealModifier += 0.15f;
                manager.ModifyRegenCounter();
                break;
            case 2:
                manager.regenCounterModifier += 3;
                manager.lifeStealModifier += 0.25f;
                manager.ModifyRegenCounter();
                break;
            case 3:
                manager.regenCounterModifier += 999;
                manager.lifeStealModifier += 0.4f;
                manager.ModifyRegenCounter();
                break;
            case 4:
                manager.regenCounterModifier += 999;
                manager.lifeStealModifier += 0.75f;
                manager.ModifyRegenCounter();
                StartCoroutine(manager.LifeDrain());
                break;
            default:
                break;
        }
    }

    public void Deactivate(int level)
    {
        switch (level)
        {
            case 1:
                manager.regenCounterModifier -= 2;
                manager.lifeStealModifier -= 0.15f;
                manager.ModifyRegenCounter();
                break;
            case 2:
                manager.regenCounterModifier -= 3;
                manager.lifeStealModifier -= 0.25f;
                manager.ModifyRegenCounter();
                break;
            case 3:
                manager.regenCounterModifier -= 999;
                manager.lifeStealModifier -= 0.4f;
                manager.ModifyRegenCounter();
                break;
            case 4:
                manager.regenCounterModifier -= 999;
                manager.lifeStealModifier -= 0.75f;
                manager.ModifyRegenCounter();
                StopCoroutine(manager.LifeDrain());
                break;
            default:
                break;
        }
    }
}
