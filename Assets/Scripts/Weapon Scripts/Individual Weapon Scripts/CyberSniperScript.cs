using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberSniperScript : MonoBehaviour, IWeapons
{
    [Header("Required Stuff")]
    [SerializeField] UIManagerScript uiManager;
    [SerializeField] StarterAssets.StarterAssetsInputs playerInput;
    [Header("Basic Stats")]
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float headshotMultiplier;
    [SerializeField] int _damageAmount;
    public int damageAmount{get => _damageAmount; set => _damageAmount = value;}
    [SerializeField] float fireRate;
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
    private int chargeAmount = 0;

    private void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    private void FixedUpdate() {
        if(playerInput.fire){
            if(!isShooting && !canNotShoot){
                isShooting = true;
                StartCoroutine("Charge");
            }
        }
        else{
            isShooting = false;
        }
    }

    public void ButtonReload()
    {
        Debug.Log("Cannot Reload Cyber Sniper");
    }

    IEnumerator Charge(){
        if(chargeAmount < 3){
            if(chargeAmount >= 1){
                if(playerStats.GetCurrentHull().meter > playerStats.GetCurrentHull().maxMeter / (12 / chargeAmount + 1)){
                    yield return new WaitForSeconds(1);
                    chargeAmount++;
                    if(isShooting)
                        StartCoroutine("Charge");
                    else
                        Shoot();
                }
                else{
                    if(isShooting){
                        yield return new WaitForEndOfFrame();
                        StartCoroutine("Charge");
                    }
                    else
                        Shoot();
                }
            }
            else{
                if(playerStats.GetCurrentHull().meter > playerStats.GetCurrentHull().maxMeter / (12)){
                    yield return new WaitForSeconds(1);
                    chargeAmount++;
                    if(isShooting)
                        StartCoroutine("Charge");
                    else
                        Shoot();
                }
            }
        }
        else{
            if(isShooting){
                yield return new WaitForEndOfFrame();
                StartCoroutine("Charge");
                }
            else
                Shoot();
        }
    }

    public void Shoot()
    {
        if(!isShooting){
            playerStats.GetCurrentHull().meter -= playerStats.GetCurrentHull().maxMeter / (12 / chargeAmount);
            playerStats.isRegening = false;
            playerStats.StopHullRegen();

            RaycastHit hit;
            Vector3 direction = FPCamera.transform.forward;
            Physics.Raycast(FPCamera.transform.position, direction, out hit, range);
            canNotShoot = true;
            StartCoroutine("CanNotShoot");
            if(hit.transform != null){
                var rb2d = hit.collider.GetComponent<Rigidbody>();
                if(rb2d){
                    rb2d.AddForceAtPosition(direction * 20, hit.point, ForceMode.Impulse);
                }
                var hitbox = hit.collider.GetComponent<EnemyHitboxScript>();
                if(hitbox){
                    if(hitbox.isHead){
                        hitbox.DealDamage(Mathf.CeilToInt(damageAmount * headshotMultiplier), direction, true);
                    }
                    else
                        hitbox.DealDamage(Mathf.CeilToInt(damageAmount), direction, false);
                }
            }
            chargeAmount = 0;
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
        //Cyber sniper does not reload
    }

    public int GetAmmoCapacity()
    {
        return -1;
    }
}
