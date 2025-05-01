using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaySelectionUIScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playSelection;
    public TMP_Text difficultyText;
    public SettingsScript settingsScript;

    private void Update() {
        switch(settingsScript.GetDifficulty()){
            case 0:
                difficultyText.text = "Current Difficulty: Easy";
                difficultyText.color = Color.white;
                break;
            case 1:
                difficultyText.text = "Current Difficulty: Medium";
                difficultyText.color = Color.white;
                break;
            case 2:
                difficultyText.text = "Current Difficulty: Hard";
                difficultyText.color = Color.yellow;
                break;
            case 3:
                difficultyText.text = "Current Difficulty: Extreme";
                difficultyText.color = Color.red;
                break;
            default:
                break;
        }
    }
    public void OnPlayButton(){
        mainMenu.SetActive(false);
        playSelection.SetActive(true);
    }

    public void OnReturnButton(){
        mainMenu.SetActive(true);
        playSelection.SetActive(false);
    }

    public void OnStartButton(){
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.SceneManagement.SceneManager.LoadScene("HangarScene");
    }

    public void OnQuitButton(){
        Application.Quit();
    }
}
