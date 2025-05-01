using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GiveWeapons : MonoBehaviour, InteractableInterface
{
    [SerializeField] RadialMenu radialMenu;
    [SerializeField] GameObject pistol;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] RemoveUI removeUI;
    [SerializeField] GameObject weaponsTutorialUI;
    [SerializeField] AiAgent[] tutorialAI;
    [SerializeField] Volume blur;
    private StarterAssets.StarterAssetsInputs starterAssetsInputs;

    private void Awake() {
        if(radialMenu.hasWeapons == false){
            this.tag = "Interactable";
        }
        starterAssetsInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    public void Interact()
    {
        StopAllCoroutines();
        blur.weight = 0;
        StartCoroutine(BlurUp());
        removeUI.RemoveUIFuction();
        weaponsTutorialUI.SetActive(true);
        Time.timeScale = 0;
        starterAssetsInputs.pauseInputs = true;
        starterAssetsInputs.cursorLocked = false;
        starterAssetsInputs.cursorInputForLook = false;
        starterAssetsInputs.SetCursorState(false);
    }

    public void EndTutorial()
    {
        StopAllCoroutines();
        blur.weight = 1;
        StartCoroutine(BlurDown());
        removeUI.ReactivateUI();
        weaponsTutorialUI.SetActive(false);
        Time.timeScale = 1;
        starterAssetsInputs.pauseInputs = false;
        starterAssetsInputs.cursorLocked = true;
        starterAssetsInputs.cursorInputForLook = true;
        starterAssetsInputs.SetCursorState(true);
        radialMenu.hasWeapons = true;
        this.tag = "Untagged";
        weaponManager.ChangeWeapons(pistol);
        foreach(AiAgent ai in tutorialAI){
            ai.stateMachine.ChangeStates(AiStateId.ChasePlayer);
        }
    }

    IEnumerator BlurUp(){
        if(blur.weight < 1){
            blur.weight += 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(BlurUp());
        }
        else
        {
            blur.weight = 1;
        }
    }

    IEnumerator BlurDown(){
        if(blur.weight > 0){
            blur.weight -= 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(BlurDown());
        }
        else
        {
            blur.weight = 0;
        }
    }

    public string Message()
    {
        return "Press F to pick up weapons";
    }
}
