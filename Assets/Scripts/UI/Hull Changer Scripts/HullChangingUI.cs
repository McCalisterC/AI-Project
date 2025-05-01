using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullChangingUI : MonoBehaviour
{
    [SerializeField] GameObject leftButtons;
    [SerializeField] GameObject RHWeaponsButtons;
    [SerializeField] GameObject hullButtons;
    [SerializeField] GameObject utilButtons;
    [SerializeField] GameObject legsButtons;
    [SerializeField] GameObject transitionImage;
    [SerializeField] StarterAssets.StarterAssetsInputs inputs;
    [SerializeField] GameObject RightButtons;


    public void OnLeftHullButton(){
        RightButtons.SetActive(true);
        RightButtons.GetComponent<DisableRightButtons>().DisableRightButton();
        
        hullButtons.SetActive(true);
    }

    public void OnLeftRHWeaponButton(){
        RightButtons.SetActive(true);
        RightButtons.GetComponent<DisableRightButtons>().DisableRightButton();
        
        RHWeaponsButtons.SetActive(true);
    }

    public void OnLeftUtilButton(){
        RightButtons.SetActive(true);
        RightButtons.GetComponent<DisableRightButtons>().DisableRightButton();
        
        utilButtons.SetActive(true);
    }

    public void OnLeftLegButton(){
        RightButtons.SetActive(true);
        RightButtons.GetComponent<DisableRightButtons>().DisableRightButton();
        
        legsButtons.SetActive(true);
    }

    public void CloseUI(){
        inputs.cursorLocked = true;
        inputs.cursorInputForLook = true;
        inputs.SetCursorState(true);
        inputs.pauseInputs = false;
        UnityEngine.Time.timeScale = 1;

        GameObject[] uiToClose = GameObject.FindGameObjectsWithTag("HullChangerUI");
        for (int i = 0; i < uiToClose.Length; i++)
        {
            uiToClose[i].SetActive(false);
        }
    }

}
