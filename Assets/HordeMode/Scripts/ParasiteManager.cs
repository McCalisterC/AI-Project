using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteManager : MonoBehaviour
{
    public int currentParasiteLevel = 1;
    public int currentAmountOfParasites = 0;
    public int maxParasiteLevel = 4;
    public GameObject[] parasites;
    private List<GameObject> parasiteList;
    private List<GameObject> usedParasites;
    private PlayerStats playerStats;
    private PlayerInventoryScript playerInventory;
    private StarterAssets.FirstPersonController playerController;
    public GameObject gun;

    [Header("Stat Modifiers")]
    public float maxHPModifier = 0;
    public float damageModifier = 0;
    public float critDamageModifier = 0;
    public float movementSpeedModifier = 0;
    public float fireRateModifier = 0;
    public float reloadSpeedModifier = 0;
    public float maxAmmoModifier = 0;
    public float maxClipSizeModifier = 0;
    public float onKillSpeedModifier = 0;
    public int regenCounterModifier = 0;
    public float lifeStealModifier = 0;

    [Header("Base Stats")]
    private float maxHP;
    private float damage;
    private float critDamage;
    private double fireRate;
    private float reloadSpeed;
    private float maxAmmo;
    private float maxClipSize;
    private float movementSpeed;
    private int regenCounter;

    [Header("Unique Modifiers")]
    public float loadedDiceModifier = 0;
    public bool russianRouletteModifier = false;
    public bool stackedDeckModifier = false;
    public int stackedDeckChance = 0;

    [Header("Debug Tools")]
    public bool debugging = false;
    public int debugParasite = 1;

    private void Awake() {
        parasiteList = new List<GameObject>(parasites);
        usedParasites = new List<GameObject>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInventoryScript>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.FirstPersonController>();

        //Obtain base stats
        maxHP = playerStats.maxHP;
        damage = gun.GetComponent<WeaponScript>().damageAmount;
        critDamage = gun.GetComponent<WeaponScript>().GetHeadshotMultiplier();
        fireRate = gun.GetComponent<WeaponScript>().GetFireRate();
        reloadSpeed = 1;
        maxAmmo = playerInventory.GetComponent<PlayerInventoryScript>().ammoInventory.GetAmmoCapacity(0);
        maxClipSize = gun.GetComponent<WeaponScript>().clipSize;
        movementSpeed = playerController.MoveSpeed;
        regenCounter = playerStats.regenCounter;
    }

    public void IncreaseParasiteLevel()
    {
        switch(currentAmountOfParasites){
            case 3:
                foreach(GameObject parasite in usedParasites)
                {
                    parasite.GetComponent<IParasite>().Deactivate(currentParasiteLevel);
                }
                currentParasiteLevel = 2;
                foreach(GameObject parasite in usedParasites)
                {
                    parasite.GetComponent<IParasite>().Activate(currentParasiteLevel);
                }
                break;
            case 6:
                foreach (GameObject parasite in usedParasites)
                {
                    parasite.GetComponent<IParasite>().Deactivate(currentParasiteLevel);
                }
                currentParasiteLevel = 3;
                foreach (GameObject parasite in usedParasites)
                {
                    parasite.GetComponent<IParasite>().Activate(currentParasiteLevel);
                }
                break;
            case 9:
                foreach (GameObject parasite in usedParasites)
                {
                    parasite.GetComponent<IParasite>().Deactivate(currentParasiteLevel);
                }
                currentParasiteLevel = 4;
                foreach (GameObject parasite in usedParasites)
                {
                    parasite.GetComponent<IParasite>().Activate(currentParasiteLevel);
                }
                break;
            default:
                break;
        }
    }

    public void ParasitePickup()
    {
        if(debugging){
            GameObject debug = parasiteList[debugParasite];
            debug.GetComponent<IParasite>().Activate(currentParasiteLevel);
            currentAmountOfParasites++;
            debugging = false;
            usedParasites.Add(debug);
            parasiteList.Remove(debug);
            IncreaseParasiteLevel();
        }
        else{
            GameObject parasite = parasiteList[Random.Range(0, parasiteList.Count)];
            parasite.GetComponent<IParasite>().Activate(currentParasiteLevel);
            currentAmountOfParasites++;
            usedParasites.Add(parasite);
            parasiteList.Remove(parasite);
            IncreaseParasiteLevel();
        }
    }

    public void ModifyHP(){
        if(maxHPModifier <= -1)
        {
            playerStats.maxHP = 1;
            playerStats.playerHP = 1;
        }
        else
        {
            playerStats.maxHP = (int)(maxHP + (maxHP * maxHPModifier));
            if (playerStats.playerHP > playerStats.maxHP)
                playerStats.playerHP = playerStats.maxHP;
            else if (playerStats.playerHP < playerStats.maxHP)
            {
                playerStats.StartRegen();
            }
        }
    }

    public void ModifyDamage(){
        gun.GetComponent<WeaponScript>().damageAmount = (int)(damage + (damage * damageModifier));
    }

    public void ModifyMovement(){
        playerController.MoveSpeed = (movementSpeed + (movementSpeed * movementSpeedModifier));
        playerController.UpdateSprintSpeed();
    }

    public void StartOnKillMovementModification(){
        StartCoroutine(ModifyMovementOnKill());
    }
    private IEnumerator ModifyMovementOnKill(){
        float temp = playerController.MoveSpeed * onKillSpeedModifier;
        playerController.MoveSpeed += temp;
        playerController.UpdateSprintSpeed();
        switch(currentParasiteLevel){
            case 1:
                yield return new WaitForSeconds(3);
                break;
            case 2:
                yield return new WaitForSeconds(4);
                break;
            case 3:
                yield return new WaitForSeconds(5);
                break;
            case 4:
                yield return new WaitForSeconds(6);
                break;
            default:
                break;
        }
        playerController.MoveSpeed -= temp;
        playerController.UpdateSprintSpeed();
    }

    public void ModifyRegenCounter(){
        playerStats.regenCounter = (regenCounter + regenCounterModifier);
    }

    public void ModifyCritDamage(){
        gun.GetComponent<WeaponScript>().SetHeadshotMultiplier((float)(critDamage + (critDamageModifier)));
    }

    public void ModifyReloadSpeed(){
        gun.GetComponent<Animator>().SetFloat("SpeedMod", reloadSpeed + (reloadSpeed * reloadSpeedModifier));
    }

    public void ModifyFireRate()
    {
        gun.GetComponent<WeaponScript>().SetFireRate((float)(fireRate + (fireRate * fireRateModifier)));
    }

    public void TryStackedDeck()
    {
        int temp = Random.Range(0, 52);
        if(temp < stackedDeckChance)
        {
            GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>().DestroyPoints();
        }
        else if(temp > 52 - stackedDeckChance)
        {
            GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>().DoublePoints();
        }
    }

    public IEnumerator LifeDrain()
    {
        playerStats.playerHP -= 5;
        yield return new WaitForSeconds(1);
        StartCoroutine(LifeDrain());
    }
}
