using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMGScript : MonoBehaviour, IWeapons
{
    [Header("Required Stuff")]
    [SerializeField] UIManagerScript uiManager;
    [SerializeField] StarterAssets.StarterAssetsInputs playerInput;
    [SerializeField] HitSparksScript hitSparksScript;
    [Header("Basic Stats")]
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float headshotMultiplier; 
    [SerializeField] int _damageAmount;
    public int damageAmount{get => _damageAmount; set => _damageAmount = value;}
    [SerializeField] float fireRate;
    [SerializeField] float inaccuracyDistance;
    [SerializeField] public int _clipSize;
    public int clipSize {get => _clipSize; set => _clipSize = value;}
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
    private Animator thisAnim;
    private bool isShooting;
    private bool isReloading;
    private bool canNotShoot;
    private int burstCounter;
    private float currentInaccuracyDistance = 0;
    private bool aimIsInaccurate;
    private bool hasStartedBurst;

    private void Awake() {
        currentAmmoAmount = clipSize;
        thisAnim = this.gameObject.GetComponent<Animator>();
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

    public void Reload(){
        currentAmmoAmount = playerStats.playerInventory.ammoInventory.ReloadAmmo(clipSize, 0, currentAmmoAmount);
        isReloading = false;
        uiManager.SetWeaponUI();
    }

    public void ButtonReload(){
        if (currentAmmoAmount != clipSize)
        {
            if (playerStats.playerInventory.ammoInventory.CheckAmmoAvailablity(0))
            {
                isReloading = true;
                thisAnim.SetTrigger("Reload");
            }
        }
    }

    public void Shoot()
    {
        if(currentAmmoAmount > 0)
        {
            if(!isReloading && isShooting){
                if(!hasStartedBurst)
                    StartCoroutine(Burst());
            }
        }
        else if(isShooting && playerStats.playerInventory.ammoInventory.CheckAmmoAvailablity(0))
        {
            if(!isReloading){
                isReloading = true;
                thisAnim.SetTrigger("Reload");
            }
        }
        
    }

    IEnumerator Burst(){
        hasStartedBurst = true;
        burstCounter++;
        currentAmmoAmount--;
        RaycastHit hit;
        Vector3 direction;
        if(burstCounter == 1){
            direction = GetShootingDirection();
            Physics.Raycast(FPCamera.transform.position, direction, out hit, range);
        }
        else{
            if(currentInaccuracyDistance < inaccuracyDistance){
                if(currentInaccuracyDistance + inaccuracyDistance / 10 > inaccuracyDistance){
                    currentInaccuracyDistance = inaccuracyDistance;
                }
                else{
                    currentInaccuracyDistance += inaccuracyDistance / 10;
                }
            }
            direction = GetShootingDirection();
            Physics.Raycast(FPCamera.transform.position, direction, out hit, range);
        }
        canNotShoot = true;
        StartCoroutine("CanNotShoot");
        uiManager.SetWeaponCurrentAmmo();
        if(hit.transform != null){
            var hitbox = hit.collider.GetComponent<EnemyHitboxScript>();
            if(hit.transform.gameObject.GetComponent<EnemyHitboxScript>() != null){
                float tempDamage;
                if(hit.distance > startFallOffRange){
                    if(hit.distance >= endFallOffRange){
                        tempDamage = damageAmount - damageAmount * (maxFallOffReduction / 100);
                    }
                    else{
                        float tempRange = endFallOffRange - startFallOffRange;
                        float temp = damageAmount - (damageAmount * (maxFallOffReduction / 100));
                        float damageDifference = damageAmount - temp;
                        tempDamage = temp + (damageDifference - (damageDifference * ((Mathf.CeilToInt(hit.distance) - startFallOffRange) / tempRange)));
                    }
                }
                else
                    tempDamage = damageAmount;
                var rb2d = hit.collider.GetComponent<Rigidbody>();
                if(rb2d){
                    rb2d.AddForceAtPosition(direction * 20, hit.point, ForceMode.Impulse);
                }
                if(hitbox.isHead){
                    hitbox.DealDamage(Mathf.CeilToInt(tempDamage * headshotMultiplier), direction, true);
                }
                else
                    hitbox.DealDamage(Mathf.CeilToInt(tempDamage), direction, false);
            }
            else{
                if(playerStats.GetCurrentHull().name == "CyberHull")
                    Instantiate(hitSparksScript.cyberHitSparksPrefab, hit.point + (hit.normal * 0.05f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                else
                    Instantiate(hitSparksScript.hitSparksPrefab, hit.point + (hit.normal * 0.05f), Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
        }
        yield return new WaitForSeconds(0.05f);
        if(burstCounter < 3)
            StartCoroutine(Burst());
        else{
            burstCounter = 0;
            if(currentInaccuracyDistance < inaccuracyDistance){
                if(currentInaccuracyDistance + inaccuracyDistance / 5 > inaccuracyDistance){
                    currentInaccuracyDistance = inaccuracyDistance;
                }
                else{
                    currentInaccuracyDistance += inaccuracyDistance / 5;
                }
            }
            if(!aimIsInaccurate){
                StartCoroutine("StabalizeAim");
                aimIsInaccurate = true;
            }
            hasStartedBurst = false;
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

    Vector3 GetShootingDirection(){
        Vector3 targetPos = FPCamera.gameObject.GetComponent<Transform>().position + FPCamera.gameObject.GetComponent<Transform>().forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-currentInaccuracyDistance, currentInaccuracyDistance),
            targetPos.y + Random.Range(-currentInaccuracyDistance, currentInaccuracyDistance),
            targetPos.z + Random.Range(-currentInaccuracyDistance, currentInaccuracyDistance)
        );

        Vector3 direction = targetPos - FPCamera.gameObject.GetComponent<Transform>().position;
        return direction.normalized;
    }

    IEnumerator StabalizeAim(){
        if(currentInaccuracyDistance > 0){
            currentInaccuracyDistance -= inaccuracyDistance / 75;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine("StabalizeAim");
        }
        else{
            currentInaccuracyDistance = 0;
            aimIsInaccurate = false;
        }
    }

    public float GetWeaponAccuracyPercentage(){
        if(currentInaccuracyDistance == 0){
            return 0;
        }
        else{
            return currentInaccuracyDistance / inaccuracyDistance;
        }
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }

    public void StopReload()
    {
        isReloading = false;
    }

    public int GetAmmoCapacity()
    {
        return playerStats.playerInventory.ammoInventory.GetAmmoCapacity(0);
    }
}
