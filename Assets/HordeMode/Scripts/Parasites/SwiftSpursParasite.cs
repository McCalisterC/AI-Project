using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftSpursParasite : MonoBehaviour, IParasite
{
    private ParasiteManager manager;
    private float speedModifierOnKill;
    private bool isActivated = false;
    public bool IsActivated{
        get{
            return isActivated;
        }
    }
    public float SpeedModifierOnKill{
        get{
            return speedModifierOnKill;
        }
    }
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("ParasiteManager").GetComponent<ParasiteManager>();
    }
    public void Activate(int level){
        switch (level)
        {
            case 1:
                manager.movementSpeedModifier -= 0.1f;
                manager.onKillSpeedModifier = 0.25f;
                isActivated = true;
                manager.ModifyMovement();
                break;
            case 2:
                manager.movementSpeedModifier -= 0.2f;
                manager.onKillSpeedModifier = 0.5f;
                isActivated = true;
                manager.ModifyMovement();
                break;
            case 3:
                manager.movementSpeedModifier -= 0.4f;
                manager.onKillSpeedModifier = 0.75f;
                isActivated = true;
                manager.ModifyMovement();
                break;
            case 4:
                manager.movementSpeedModifier -= 0.75f;
                manager.onKillSpeedModifier = 2f;
                isActivated = true;
                manager.ModifyMovement();
                break;
        }
    }

    public void Deactivate(int level){
        switch (level)
        {
            case 1:
                manager.movementSpeedModifier += 0.1f;
                manager.onKillSpeedModifier = 0f;
                isActivated = false;
                manager.ModifyMovement();
                break;
            case 2:
                manager.movementSpeedModifier += 0.2f;
                manager.onKillSpeedModifier = 0f;
                isActivated = false;
                manager.ModifyMovement();
                break;
            case 3:
                manager.movementSpeedModifier += 0.4f;
                manager.onKillSpeedModifier = 0f;
                isActivated = false;
                manager.ModifyMovement();
                break;
            case 4:
                manager.movementSpeedModifier += 0.75f;
                manager.onKillSpeedModifier = 0f;
                isActivated = false;
                manager.ModifyMovement();
                break;
        }
    }

}
