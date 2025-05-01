using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeLauncherScript : MonoBehaviour, IWeapons
{
    [Header("Required Stuff")]
    [SerializeField] UIManagerScript uiManager;
    [SerializeField] StarterAssets.StarterAssetsInputs playerInput;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] GrenadeLauncherVFXScript grenadeLauncherVFXScript;
    [SerializeField] GameObject shootPointTransform;
    [Header("Basic Stats")]
    [SerializeField] Camera FPCamera;
    [SerializeField] float fireRate;
    [SerializeField] float force = 15f;
    [SerializeField] int _damageAmount;
    public int damageAmount{get => _damageAmount; set => _damageAmount = value;}
    [SerializeField] public int _maxAmmoAmount;
    public int clipSize {get => _maxAmmoAmount; set => _maxAmmoAmount = value;}
    [SerializeField] public int _currentAmmoAmount;
    public int currentAmmoAmount {get => _currentAmmoAmount; set => _currentAmmoAmount = value;}
    [SerializeField] public float _startFallOffRange;
    public float startFallOffRange {get => _startFallOffRange; set => _startFallOffRange = value;}

    [SerializeField] public float _endFallOffRange;
    public float endFallOffRange {get => _endFallOffRange; set => _endFallOffRange = value;}

    [SerializeField] public float _maxFallOffReduction;
    public float maxFallOffReduction {get => _maxFallOffReduction; set => _maxFallOffReduction = value;}
    [SerializeField] public int _weaponSpread = 200;
    public int weaponSpread {get => _weaponSpread; set => _weaponSpread = value;}
    PlayerStats playerStats;
    private bool isShooting;
    private bool canNotShoot;

    private void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    private void FixedUpdate() {
        if(playerInput.fire){
            if(!isShooting && !canNotShoot){
                isShooting = true;
                Shoot();
            }
        }
        else{
            isShooting = false;
        }
    }

    public void ButtonReload()
    {
        Debug.Log("Cannot Reload Grenade Launcher");
    }

    public void Shoot()
    {
        if(isShooting){
            if(playerStats.GetCurrentHull().meter - playerStats.GetCurrentHull().maxMeter / 6 > 0){
                playerStats.isRegening = false;
                playerStats.StopHullRegen();
                playerStats.GetCurrentHull().meter -= playerStats.GetCurrentHull().maxMeter / 6;
                Ray r = FPCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                Vector3 dir = r.GetPoint(1) - r.GetPoint(0);
                GameObject tempFrag = Instantiate(grenadePrefab, shootPointTransform.transform.position, Quaternion.LookRotation(dir));
                tempFrag.GetComponent<Rigidbody>().velocity = tempFrag.transform.forward * force;
                if(playerStats.GetCurrentHull().name == "CyberHull")
                    tempFrag.GetComponent<GrenadeLauncherExplosionScript>().explosionVFX = grenadeLauncherVFXScript.cyberPrefab;
                canNotShoot = true;
                StartCoroutine(CanNotShoot());
            }
        }
        
    }

    IEnumerator CanNotShoot(){
        yield return new WaitForSeconds((float)(1.0/fireRate));
        canNotShoot = false;
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
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
        //Grenade launcher does not reload
    }

    public int GetAmmoCapacity()
    {
        return -1;
    }
}
