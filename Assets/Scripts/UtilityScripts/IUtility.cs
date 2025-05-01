using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUtility
{
    public bool isOnCooldown{get; set;}
    public void OnUtilPress();
    public void DeactivateThis();
    public GameObject GetGameObject();
}
