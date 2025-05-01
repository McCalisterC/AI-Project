using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberHullScript : MonoBehaviour, IHullInterface
{
    [SerializeField] Color color;

    public string _name;
    new string name{get => _name; set => _name = value;}

    int _meter;
    public int meter { get => _meter; set => _meter = value; }

    int _maxMeter = 100;
    public int maxMeter { get => _maxMeter; set => _maxMeter = value; }

    public void GainMeter()
    {
        if (meter + 5 >= maxMeter){
            meter = maxMeter;
        }
        else
            meter += 5;
    }

    private void Awake() {
        meter = 0;
    }
    
    public void DisableHull(){
        this.gameObject.SetActive(false);
    }

    public Color GetColor(){
        return color;
    }
}
