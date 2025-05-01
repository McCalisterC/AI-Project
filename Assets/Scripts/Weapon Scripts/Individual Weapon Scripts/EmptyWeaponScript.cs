using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeaponScript : MonoBehaviour, IWeapons
{
    public int clipSize { get => 0; set => Debug.Log("no"); }
    public int currentAmmoAmount { get => 0; set => Debug.Log("no"); }
    [SerializeField] int _damageAmount;
    public int damageAmount{get => _damageAmount; set => _damageAmount = value;}
    [SerializeField] public float _startFallOffRange;
    public float startFallOffRange {get => _startFallOffRange; set => _startFallOffRange = value;}

    [SerializeField] public float _endFallOffRange;
    public float endFallOffRange {get => _endFallOffRange; set => _endFallOffRange = value;}

    [SerializeField] public float _maxFallOffReduction;
    public float maxFallOffReduction {get => _maxFallOffReduction; set => _maxFallOffReduction = value;}
    [SerializeField] public int _weaponSpread = 200;
    public int weaponSpread {get => _weaponSpread; set => _weaponSpread = value;}

    public void ButtonReload()
    {
        Debug.Log("Cannot Reload Empty Weapon Slot");
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void Shoot()
    {
        Debug.Log("Cannot Shoot Empty Weapon Slot");
    }
    public GameObject GetGameObject(){
        return this.gameObject;
    }
    public float GetWeaponAccuracyPercentage(){
        return 0;
    }
    public bool GetIsReloading()
    {
        //Can't reload
        return false;
    }

    public void StopReload()
    {
        //Empty does not reload
    }

    public int GetAmmoCapacity()
    {
        return -1;
    }
}
