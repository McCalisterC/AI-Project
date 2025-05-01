using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullHullScript : MonoBehaviour, IHullInterface
{
    [SerializeField] string _name;

    [SerializeField] Color color;

    public new string name
    {
        get => _name;
        set => _name = value;
    }

    private int shield;

    public int meter
    {
        get => shield;
        set => shield = value;
    }

    private int maxShield = 0;

    public int maxMeter
    {
        get => maxShield;
        set => maxShield = value;
    }
    public void GainMeter()
    {
        //Do nothing
    }

    private void Awake()
    {
        shield = 0;
    }

    public void DisableHull()
    {
        this.gameObject.SetActive(false);
    }

    public Color GetColor()
    {
        return color;
    }
}
