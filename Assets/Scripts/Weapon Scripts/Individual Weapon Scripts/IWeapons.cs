using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapons
{
    int clipSize{get; set;}
    int currentAmmoAmount{get; set;}
    int damageAmount{get; set;}
    float startFallOffRange{get; set;}
    float endFallOffRange{get; set;}
    float maxFallOffReduction{get; set;}
    int weaponSpread{get; set;}
    public void Shoot();
    public void ButtonReload();
    public void Disable();
    public GameObject GetGameObject();
    public float GetWeaponAccuracyPercentage();
    public bool GetIsReloading();
    public void StopReload();
    public int GetAmmoCapacity();
}
