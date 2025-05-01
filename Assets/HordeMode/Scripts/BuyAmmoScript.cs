using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAmmoScript : MonoBehaviour, InteractableInterface
{
    private PointManager pointManager;
    private PlayerInventoryScript playerInventory;
    public int ammoCost = 2000;
    private string message;
    public string GetMessage() { return message; }

    private void Awake()
    {
        pointManager = GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInventoryScript>();
        message = "Press F to buy ammo for " + ammoCost + " points";
    }
    public void Interact()
    {
        if(pointManager.GetPoints() > ammoCost)
        {
            pointManager.RemovePoints(ammoCost);
            playerInventory.ammoInventory.RefillPistolAmmo();
        }
    }

    public string Message()
    {
        return message;
    }
}
