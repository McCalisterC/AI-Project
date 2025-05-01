using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHullScript : MonoBehaviour, IHullInterface
{
    [SerializeField] string _name;

    [SerializeField] Color color;

    public new string name{
        get => _name;
        set => _name = value;
    }

    private int shield;

    public int meter{
        get => shield;
        set => shield = value;
    }

    private int maxShield = 100;

    public int maxMeter{
        get => maxShield;
        set => maxShield = value;
    }
    public void GainMeter(){
        if(shield + 5 >= maxShield)
            shield = maxShield;
        else
            shield += 5;
    }

    private void Awake() {
        shield = maxShield;
    }

    public void DisableHull(){
        this.gameObject.SetActive(false);
    }

    public Color GetColor(){
        return color;
    }
}
