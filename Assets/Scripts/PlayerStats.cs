using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 150;
    public int playerHP = 150;
    public int sprintMeter = 100;
    public bool isRegening = false;
    public int regenCounter = 5;
    public bool isRegeningSprint = false;
    public bool isCrouching = false;
    public TextMeshProUGUI hpText;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider meterSlider;
    [SerializeField] Slider sprintSlider;
    [SerializeField] GameObject hullParent;
    [SerializeField] GameObject legParent;
    [SerializeField] StarterAssets.FirstPersonController FPController;
    [SerializeField] KnifeScript knife;
    public PlayerInventoryScript playerInventory;
    private IHullInterface currentHull;
    private ILegs currentLegs;
    private int legCounter;
    public bool hasStartedCounting;

    private void Awake(){
        hpSlider.maxValue = maxHP;
        currentHull = hullParent.GetComponentInChildren<IHullInterface>();
        currentLegs = legParent.GetComponentInChildren<ILegs>();
        if(meterSlider != null)
            meterSlider.maxValue = currentHull.maxMeter;
        if(sprintSlider != null)
            sprintSlider.maxValue = sprintMeter;
        StartCoroutine("CheckSprint");
    }

    private void Update() {
        if(currentHull.name == "Base Hull" && currentHull.meter < currentHull.maxMeter && !hasStartedCounting){
            hasStartedCounting = true;
            StartCoroutine("CountUntilRegen");
        }
        if(!FPController.CheckIfSprinting() && !isRegeningSprint){
			isRegeningSprint = true;
            StartCoroutine("StartRegenSprint");
		}
        if(FPController.CheckIfSprinting()){
            StopCoroutine("StartRegenSprint");
            StopCoroutine("RegenSprint");
            isRegeningSprint = false;
        }
        if(sprintMeter > 0 && !isCrouching){
            if(FPController.canSprint != true){
                FPController.canSprint = true;
            }
        }
        if(currentLegs.name == "Energy Sprinters"){
            if(currentHull.meter <= 0){
                FPController.canSprint = false;
            }
            else if(!isCrouching){
                FPController.canSprint = true;
            }
        }
        hpText.SetText("Health: " + playerHP);
        hpSlider.value = playerHP;
        if(meterSlider != null)
            meterSlider.value = currentHull.meter;
        if(sprintSlider != null)
            sprintSlider.value = sprintMeter;
    }

    public void StartTakeDamage(int damage){
        if(currentHull.name == "Base Hull")
            StopCoroutine("HullRegen");
        StopCoroutine("Regen");
        StopCoroutine("CountUntilRegen");
        hasStartedCounting = true;
        StartCoroutine("CountUntilRegen");
        StartCoroutine(TakeDamage(damage));
    }

    public IEnumerator TakeDamage(int damage){
        isRegening = false;
        StopCoroutine("Regen");
        if(currentHull.name == "Base Hull"){
            if(currentHull.meter - damage >= 0)
                currentHull.meter -= damage;
            else if(currentHull.meter == 0){
                playerHP -= damage;
            }
            else if(currentHull.meter - damage < 0){
                int temp = damage - currentHull.meter;
                currentHull.meter = 0;
                playerHP -= temp;
            }
        }
        else
            playerHP -= damage;

        yield return new WaitForSeconds(5f);

        isRegening = true;
    }

    public void StartRegen(){
        if(!isRegening)
            StartCoroutine("Regen");
    }

    IEnumerator CountUntilRegen(){
        yield return new WaitForSeconds(regenCounter);
        isRegening = true;
        StartCoroutine("Regen");
    }

    public IEnumerator Regen(){
        if(currentHull.name == "Base Hull" && !(playerHP < maxHP)){
            StartCoroutine("HullRegen");
        }
        else if (playerHP < maxHP){
            if(playerHP < maxHP && playerHP + 5 < maxHP){
                playerHP += 5;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine("Regen");
            }
            else if((playerHP + 5 >= maxHP)){
                playerHP = maxHP;
                if(currentHull.name == "Base Hull")
                    StartCoroutine("Regen");
                else
                    isRegening = false;
            }
        }
    }

    public void Heal(int amount){
        if(playerHP + amount <= maxHP)
            playerHP += amount;
        else
            playerHP = maxHP;
    }

    public IEnumerator HullRegen(){
        int currentShield = currentHull.meter;
        int maxShield = currentHull.maxMeter;
        if(playerHP == maxHP && currentShield < maxShield){
            if(currentShield < maxShield && currentShield + 5 < maxShield){
                currentHull.GainMeter();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine("HullRegen");
            }
            else if(currentShield + 5 >= maxShield){
                currentHull.GainMeter();
                isRegening = false;
                hasStartedCounting = false;
            }
        }
    }

    public void SetCurrentHull(GameObject hull){
        currentHull.DisableHull();
        hull.SetActive(true);
        currentHull = hull.GetComponentInChildren<IHullInterface>();
        meterSlider.maxValue = currentHull.maxMeter;
        knife.SetEmissiveColor();
    }

    public IHullInterface GetCurrentHull(){
        return currentHull;
    }

    public void SetCurrentLegs(GameObject newLegs){
        currentLegs = newLegs.GetComponent<ILegs>();
    }

    public ILegs GetCurrentLegs(){
        return currentLegs;
    }

    public void StopRegen(){
        StopCoroutine("Regen");
        StopCoroutine("CountUntilRegen");
        hasStartedCounting = false;
    }

    public void StopHullRegen(){
        StopCoroutine("HullRegen");
        StopCoroutine("CountUntilRegen");
        hasStartedCounting = false;
    }

    IEnumerator CheckSprint(){
        yield return new WaitForSeconds(0.05f);
        if(sprintMeter <= 0){
            FPController.SetSprintFullRecharge(true);
            FPController.canSprint = false;
        }
        else if(FPController.CheckIfSprinting()){
            if(currentLegs.name == "Energy Sprinters"){
                legCounter++;
                StopHullRegen();
                StopCoroutine("CountUntilRegen");
                hasStartedCounting = false;
            }
            sprintMeter -= 1;
        }
        if(legCounter == 10){
            legCounter = 0;
            currentHull.meter--;
        }
        StartCoroutine("CheckSprint");
    }

    IEnumerator StartRegenSprint(){
        yield return new WaitForSeconds(2);
        if(isRegeningSprint){
            legCounter = 0;
            StartCoroutine("RegenSprint");
        }
    }

    IEnumerator RegenSprint(){
        yield return new WaitForSeconds(0.01f);
        if(sprintMeter == 100){
            FPController.SetSprintFullRecharge(false);
            isRegeningSprint = false;
        }
        else{
            sprintMeter += 1;
            StartCoroutine("RegenSprint");
        }
    }

    public void SetHealthUI(){
        hpSlider.maxValue = maxHP;
    }
}
