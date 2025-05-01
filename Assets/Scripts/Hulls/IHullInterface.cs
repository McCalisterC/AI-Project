using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHullInterface
{
    public string name{get; set;}
    public int meter{get; set;}
    public int maxMeter{get; set;}
    public Color GetColor();
    public void GainMeter();
    public void DisableHull();
}
