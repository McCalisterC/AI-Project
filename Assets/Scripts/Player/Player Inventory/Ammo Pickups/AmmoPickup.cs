using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int ammoAmount = 15;
    [SerializeField] int ammoType = 0;
    [SerializeField] float distanceToPlayer = 5f;
    private PlayerInventoryScript playerInventory;

    private void Awake()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInventoryScript>();
    }

    // Function that detects if the player is close to the ammo pickup
    // If player is close and can pick up ammo, the ammo floats towards the player
    // Else the ammo stays in place
    private void FloatAmmoTowardsPlayer()
    {
        if (Vector3.Distance(transform.position, playerInventory.transform.position) < distanceToPlayer)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            transform.position = Vector3.MoveTowards(transform.position, playerInventory.transform.position, 0.5f);
        }
    }

    private void Update()
    {
        if (playerInventory.ammoInventory.CheckAmmoMaxCapacity(ammoType) == false)
        {
            FloatAmmoTowardsPlayer();
        }
        else
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    // Function to increase the players ammo amount when ammo is picked up
    // ammoType key: 0 = Light, 1 = Medium, 2 = Heavy
    // This function is called when the player collides with the ammo pickup
    // If the player is already at max ammo, the player will not pick up the ammo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            if(playerInventory.ammoInventory.CheckAmmoMaxCapacity(ammoType) == false)
            {
                playerInventory.ammoInventory.AmmoPickUp(ammoAmount, ammoType);
                Destroy(gameObject);
            }
        }
    }
}
