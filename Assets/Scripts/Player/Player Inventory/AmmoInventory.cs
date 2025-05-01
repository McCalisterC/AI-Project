using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    [Header("Max Ammo Capacities")]
    [SerializeField] int maxLightAmmoCapacity;
    [SerializeField] int maxMediumAmmoCapacity;
    [SerializeField] int maxHeavyAmmoCapacity;

    [Header("Current Ammo Capacities")]
    [SerializeField] int currentLightAmmoAmount;
    [SerializeField] int currentMediumAmmoAmount;
    [SerializeField] int currentHeavyAmmoAmount;

    [Header("Ammo UI")]
    [SerializeField] UIManagerScript ammoUI;
    
    void Start()
    {
        //Ammo is set to maximum at the start of the game. May have to be changed when a save system is implemented.
        currentLightAmmoAmount = maxLightAmmoCapacity;
        currentMediumAmmoAmount = maxMediumAmmoCapacity;
        currentHeavyAmmoAmount = maxHeavyAmmoCapacity;
    }

    //Function to increase the players ammo amount when ammo is picked up
    //ammoType key: 0 = Light, 1 = Medium, 2 = Heavy
    public void AmmoPickUp(int amount, int ammoType)
    {
        switch (ammoType) 
        {
            case 0:
                if (amount + currentLightAmmoAmount > maxLightAmmoCapacity)
                {
                    currentLightAmmoAmount = maxLightAmmoCapacity;
                    ammoUI.SetWeaponUI();
                }
                else
                {
                    currentLightAmmoAmount += amount;
                    ammoUI.SetWeaponUI();
                }
                break;
            case 1:
                if (amount + currentMediumAmmoAmount > maxMediumAmmoCapacity)
                {
                    currentMediumAmmoAmount = maxMediumAmmoCapacity;
                    ammoUI.SetWeaponUI();
                }
                else
                {
                    currentMediumAmmoAmount += amount;
                    ammoUI.SetWeaponUI();
                }
                break;
            case 2:
                if (amount + currentHeavyAmmoAmount > maxHeavyAmmoCapacity)
                {
                    currentHeavyAmmoAmount = maxHeavyAmmoCapacity;
                    ammoUI.SetWeaponUI();
                }
                else
                {
                    currentHeavyAmmoAmount += amount;
                    ammoUI.SetWeaponUI();
                }
                break;
            default:
                break;
        }
    }

    //Function to reduce the player's ammo amount when reload is complete.
    //ammoType key: 0 = Light, 1 = Medium, 2 = Heavy
    public int ReloadAmmo(int clipSize, int ammoType, int currentAmmoAmount)
    {
        switch (ammoType)
        {
            case 0:
                if (currentLightAmmoAmount - clipSize < 0)
                {
                    int temp = currentLightAmmoAmount;
                    currentLightAmmoAmount = 0;
                    return temp;
                }
                else
                {
                    currentLightAmmoAmount -= clipSize - currentAmmoAmount;
                    return clipSize;
                }
            case 1:
                if (currentMediumAmmoAmount - clipSize < 0)
                {
                    int temp = currentMediumAmmoAmount;
                    currentMediumAmmoAmount = 0;
                    return temp;
                }
                else
                {
                    currentMediumAmmoAmount -= clipSize - currentAmmoAmount;
                    return clipSize;
                }
            case 2:
                if (currentHeavyAmmoAmount - clipSize < 0)
                {
                    int temp = currentHeavyAmmoAmount;
                    currentHeavyAmmoAmount = 0;
                    return temp;
                }
                else
                {
                    currentHeavyAmmoAmount -= clipSize - currentAmmoAmount;
                    return clipSize;
                }
            default:
                return 0;
        }
    }

    //Function to check if ammo is available for reload.
    //Return true means there is ammo available, false means there is not.
    public bool CheckAmmoAvailablity(int ammoType)
    {
        switch (ammoType)
        {
            case 0:
                if (currentLightAmmoAmount <= 0)
                    return false;
                else
                    return true;
            case 1:
                if (currentMediumAmmoAmount <= 0)
                    return false;
                else
                    return true;
            case 2:
                if (currentHeavyAmmoAmount <= 0)
                    return false;
                else
                    return true;
            default:
                return false;
        }
    }

    //Function to check if ammo is at max capacity.
    //Return true means ammo is at max capacity, false means it is not.
    public bool CheckAmmoMaxCapacity(int ammoType)
    {
        switch (ammoType) 
        { 
            case 0:
                if (currentLightAmmoAmount >= maxLightAmmoCapacity)
                    return true;
                else
                    return false;
            case 1:
                if (currentMediumAmmoAmount >= maxMediumAmmoCapacity)
                    return true;
                else
                    return false;
            case 2:
                if (currentHeavyAmmoAmount >= maxHeavyAmmoCapacity)
                    return true;
                else
                    return false;
            default:
                return false;
        }

    }

    public int GetAmmoCapacity(int ammoType)
    {
        switch (ammoType)
        {
            case 0:
                return currentLightAmmoAmount;
            case 1:
                return currentMediumAmmoAmount;
            case 2:
                return currentHeavyAmmoAmount;
            default:
                return 0;
        }
    }

    //Horde mode specific functions
    public void RefillPistolAmmo()
    {
        currentLightAmmoAmount = maxLightAmmoCapacity;
    }
}
