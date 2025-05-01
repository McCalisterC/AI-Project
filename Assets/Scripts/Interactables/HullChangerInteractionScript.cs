using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullChangerInteractionScript : MonoBehaviour, InteractableInterface
{
    [SerializeField] GameObject defaultWeapon;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] StarterAssets.StarterAssetsInputs inputs;
    [SerializeField] GameObject[] hullChangerUIElements;
    public void Interact()
    {
        inputs.cursorLocked = false;
        inputs.cursorInputForLook = false;
        inputs.SetCursorState(false);
        inputs.pauseInputs = true;
        weaponManager.ChangeWeapons(defaultWeapon);
        UnityEngine.Time.timeScale = 0;

        //Open Hull Changer UI
        for (int i = 0; i < hullChangerUIElements.Length; i++)
        {
            hullChangerUIElements[i].SetActive(true);
        }
    }

    public string Message()
    {
        return "Press F to change hull";
    }
}
