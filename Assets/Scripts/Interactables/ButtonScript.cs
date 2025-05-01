using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, InteractableInterface
{
    public string sceneToLoad;
    public bool buttonActive;

    [SerializeField]
    GameObject transition;
    private StarterAssets.StarterAssetsInputs playerInputs;

    private void Start() {
        playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    public void Interact(){
        LoadLevel();
    }
    public void LoadLevel(){
        if(buttonActive){
            StartCoroutine("Transition");
        }
    }

    public void SetButtonToActive(){
        buttonActive = true;
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    IEnumerator Transition(){
        playerInputs.pauseInputs = true;
        transition.GetComponent<Animator>().SetTrigger("Transition");
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    public string Message()
    {
        return null;
    }
}
