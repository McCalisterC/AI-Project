using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour, IWeapons
{
    [Header("Required Stuff")]
    [SerializeField] UIManagerScript uiManager;
    [SerializeField] StarterAssets.StarterAssetsInputs playerInput;
    [SerializeField] HitSparksScript hitSparksScript;
    PlayerStats playerStats;
    [Header("Basic Stats")]
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float headshotMultiplier;
    public float GetHeadshotMultiplier() { return headshotMultiplier; }
    public void SetHeadshotMultiplier(float value) { headshotMultiplier = value; }
    [SerializeField] int _damageAmount;
    public int damageAmount{get => _damageAmount; set => _damageAmount = value;}
    [SerializeField] double fireRate;
    public double GetFireRate() { return fireRate; }
    public void SetFireRate(double value) { fireRate = value; }
    [Tooltip("How Many Bullets Can Be Shot Per Second")]
    [SerializeField] public int _clipSize;
    public int clipSize {get => _clipSize; set => _clipSize = value;}
    [SerializeField] float inaccuracyDistance = 5f;
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

    private Animator thisAnim;
    private bool isReloading;
    private bool isShooting;
    private bool canNotShoot;
    private float currentInaccuracyDistance = 0;
    private bool aimIsInaccurate = false;

    [Header("Ammo Type")]
    [Tooltip("Ammo Type Key: 0 = Light, 1 = Medium, 2 = Heavy")]
    [SerializeField] int ammoType;

    [Header("Automatic")]
    [SerializeField] bool isAutomatic;
    bool hasFanTheHammer = false;
    public void SetHasFanTheHammer(bool value) { hasFanTheHammer = value; }
    int fanTheHammerAmmoCost = 0;
    public void SetFanTheHammerAmmoCost(int value) { fanTheHammerAmmoCost = value; }
    bool hasStartedBurst = false;
    int burstCounter = 0;

    [Header("Shotgun")]
    [SerializeField] bool isShotgun;
    [SerializeField] int pelletsPerShot = 6;

    public void Awake(){
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

    public void Shoot(){
        if(hasFanTheHammer)
        {
            if(!isReloading && isShooting){
                if(!hasStartedBurst)
                    StartCoroutine(FanTheHammer());
            }
        }
        else
        {
            if(currentAmmoAmount != 0)
            {
                if(!isReloading && isShooting){
                    currentAmmoAmount--;

                    if (isShotgun){
                        canNotShoot = true;
                        StartCoroutine("CanNotShoot");
                        uiManager.SetWeaponCurrentAmmo();
                        for (int i = 0; i < pelletsPerShot; i++)
                        {
                            RaycastHit hit;
                            Vector3 directionShotgun = GetShootingDirectionShotgun();
                            Physics.Raycast(FPCamera.transform.position, directionShotgun, out hit, range);
                            if(hit.transform != null){ 
                                var hitbox = hit.collider.GetComponent<EnemyHitboxScript>();
                                if(hitbox){
                                    float tempDamage;
                                    if(hit.distance > startFallOffRange){
                                        if(hit.distance >= endFallOffRange){
                                            tempDamage = _damageAmount - _damageAmount * (maxFallOffReduction / 100);
                                        }
                                        else{
                                            float tempRange = endFallOffRange - startFallOffRange;
                                            float temp = _damageAmount - (_damageAmount * (maxFallOffReduction / 100));
                                            float damageDifference = _damageAmount - temp;
                                            tempDamage = temp + (damageDifference - (damageDifference * ((Mathf.CeilToInt(hit.distance) - startFallOffRange) / tempRange)));
                                        }
                                    }
                                    else
                                        tempDamage = _damageAmount;
                                    hit.collider.GetComponent<EnemyHitboxScript>().DealDamage(Mathf.CeilToInt(tempDamage), directionShotgun, false);

                                    var rb2d = hit.collider.GetComponent<Rigidbody>();
                                    if(rb2d){
                                        rb2d.AddForceAtPosition(directionShotgun * 20, hit.point, ForceMode.Impulse);
                                    }
                                    if(hitbox.isHead){
                                        hitbox.DealDamage(Mathf.CeilToInt(tempDamage * headshotMultiplier), directionShotgun, true);
                                    }
                                    else
                                        hitbox.DealDamage(Mathf.CeilToInt(tempDamage), directionShotgun, false);
                                    
                                }
                                else{
                                    if(playerStats.GetCurrentHull().name == "CyberHull")
                                        Instantiate(hitSparksScript.cyberHitSparksPrefab, hit.point + (hit.normal * 0.05f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                                    else
                                        Instantiate(hitSparksScript.hitSparksPrefab, hit.point + (hit.normal * 0.05f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                                }
                            }
                        }
                    }
                    else{
                        RaycastHit hit;
                        Vector3 direction = GetShootingDirection();
                        Physics.Raycast(FPCamera.transform.position, direction, out hit, range);
                        canNotShoot = true;
                        StartCoroutine("CanNotShoot");
                        uiManager.SetWeaponCurrentAmmo();
                        if(hit.transform != null){
                            var hitbox = hit.collider.GetComponent<EnemyHitboxScript>();
                            if(hitbox){
                                float tempDamage;
                                if(hit.distance > startFallOffRange){
                                    if(hit.distance >= endFallOffRange){
                                        tempDamage = _damageAmount - _damageAmount * (maxFallOffReduction / 100);
                                    }
                                    else{
                                        float tempRange = endFallOffRange - startFallOffRange;
                                        float temp = _damageAmount - (_damageAmount * (maxFallOffReduction / 100));
                                        float damageDifference = _damageAmount - temp;
                                        tempDamage = temp + (damageDifference - (damageDifference * ((Mathf.CeilToInt(hit.distance) - startFallOffRange) / tempRange)));
                                    }
                                }
                                else
                                    tempDamage = _damageAmount;

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
                    }
                    if(isShooting && isAutomatic){
                        StartCoroutine("RapidFire");
                    }

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
                }
            }
            else if(isShooting && playerStats.playerInventory.ammoInventory.CheckAmmoAvailablity(ammoType))
            {
                if(!isReloading){
                    isReloading = true;
                    thisAnim.SetTrigger("Reload");
                }
            }
        }
        
    }

    public void Reload(){
        currentAmmoAmount = playerStats.playerInventory.ammoInventory.ReloadAmmo(clipSize, ammoType, currentAmmoAmount);
        isReloading = false;
        uiManager.SetWeaponUI();
    }

    public void ButtonReload(){
        if(currentAmmoAmount != clipSize){
            if (playerStats.playerInventory.ammoInventory.CheckAmmoAvailablity(ammoType))
            {
                isReloading = true;
                thisAnim.SetTrigger("Reload");
            }
        }
    }
    IEnumerator RapidFire(){
        if(playerInput.fire){
            yield return new WaitForSeconds((float)(1.0/fireRate));
            Shoot();
        }
    }

    IEnumerator CanNotShoot(){
        yield return new WaitForSeconds((float)(1.0/fireRate));
        canNotShoot = false;
    }

    //Method to randomize shooting direction (right now only used for the shotgun)
    Vector3 GetShootingDirectionShotgun(){
        Vector3 targetPos = FPCamera.gameObject.GetComponent<Transform>().position + FPCamera.gameObject.GetComponent<Transform>().forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance)
        );

        Vector3 direction = targetPos - FPCamera.gameObject.GetComponent<Transform>().position;
        return direction.normalized;
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

    public void Disable(){
        this.gameObject.SetActive(false);
    }

    public GameObject GetGameObject(){
        return this.gameObject;
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
        if(isShotgun){
            return 1;
        }
        else{
            if(currentInaccuracyDistance == 0){
                return 0;
            }
            else{
                return currentInaccuracyDistance / inaccuracyDistance;
            }
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
        return playerStats.playerInventory.ammoInventory.GetAmmoCapacity(ammoType);
    }

    IEnumerator FanTheHammer(){
        if(currentAmmoAmount > 0){
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
            if(burstCounter < fanTheHammerAmmoCost && currentAmmoAmount > 0)
                StartCoroutine(FanTheHammer());
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
        
    }
}
