using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponWheelScriptableObject", order = 1)]
public class WeaponWheelScriptableObject : ScriptableObject
{
    public string label;
    public Sprite icon;
}
