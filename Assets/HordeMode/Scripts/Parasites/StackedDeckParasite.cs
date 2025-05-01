using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedDeckParasite : MonoBehaviour, IParasite
{
    //Stacked deck gives the player a 1/52 chance to double points, but also gives the player a 1/52 chance to lose half points
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
                manager.stackedDeckModifier = true;
                manager.stackedDeckChance = 1;
                break;
            case 2:
                manager.stackedDeckModifier = true;
                manager.stackedDeckChance = 2;
                break;
            case 3:
                manager.stackedDeckModifier = true;
                manager.stackedDeckChance = 3;
                break;
            case 4:
                manager.stackedDeckModifier = true;
                manager.stackedDeckChance = 4;
                break;
            default:
                break;
        }
    }

    public void Deactivate(int level)
    {
        manager.stackedDeckModifier = false;
        manager.stackedDeckChance = 0;
    }
}
