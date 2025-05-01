using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySprintersScript : MonoBehaviour, ILegs
{
    public string _name;
    new string name{get => _name; set => _name = value;}
    [SerializeField] StarterAssets.FirstPersonController FPController;

    private void Awake() {
        FPController = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.FirstPersonController>();
    }
    public void ModifyMovement(){
        FPController.SprintSpeedPercentage += 0.8f;
        FPController.UpdateSprintSpeed();
    }

    public void DisableLegs(){
        FPController.SprintSpeedPercentage -= 0.8f;
        FPController.UpdateSprintSpeed();
        this.gameObject.SetActive(false);
    }
}
