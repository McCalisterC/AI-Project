using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    [SerializeField] StarterAssets.FirstPersonController firstPersonController;
    [SerializeField] StarterAssets.StarterAssetsInputs playerInput;
    [SerializeField] PlayerStats playerStats;
    public bool isCrouching = false;
    private Animator thisAnim;
    private bool _hasStartedCrouching = false;
    private bool _hasStartedRising = false;
    private bool canCrouch = true;

    private void Awake() {
        thisAnim = this.GetComponent<Animator>();

    }
    private void FixedUpdate() {
        if(playerInput.crouch && canCrouch){
            Crouch();
            canCrouch = false;
        }
    }

    public void Crouch(){
        if(!_hasStartedCrouching && !_hasStartedRising){
                isCrouching = !isCrouching;
                if(isCrouching == true){
                    playerStats.isCrouching = true;
                    firstPersonController.canSprint = false;
                }
                else{
                    playerStats.isCrouching = false;
                }
                thisAnim.SetBool("isCrouching", isCrouching);
            }
    }
    public void ToggleCrouch(){
        _hasStartedCrouching = !_hasStartedCrouching;
    }

    public void ToggleRise(){
        _hasStartedRising = !_hasStartedRising;
    }

    public void CanCrouchToTrue(){
        canCrouch = true;
    }
}
