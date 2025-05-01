using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteractionScript : MonoBehaviour
{
    public Camera FPCamera;
    public UIManagerScript uiManager;
    public bool canInteract = true;
    public float interactRange = 1f;

    private void Update() {
        CheckIfHitInteractable();
    }

    public void Interact(){
        if(canInteract){
            RaycastHit hit;
            Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, interactRange);

            GameObject other = hit.transform.gameObject;

            try{
                var otherScript = other.gameObject.GetComponent<InteractableInterface>();
                otherScript.Interact();
            }
            catch{

            }
        }
    }

    public void CheckIfHitInteractable(){
        RaycastHit hit;
        Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, interactRange);

        GameObject other = this.gameObject;
        if(hit.transform != null){
            other = hit.transform.gameObject;
        }

        if(other.tag == "Interactable"){
            if(other.GetComponent<InteractableInterface>().Message() != null)
            {
                uiManager.SetInteractionText(other.GetComponent<InteractableInterface>().Message());
            }
            else
            {
                uiManager.SetInteractionText("Press F to interact");
            }
            uiManager.ActivateInteractionText();
        }
        else
            uiManager.DeactivateInteractionText();
    }
}
