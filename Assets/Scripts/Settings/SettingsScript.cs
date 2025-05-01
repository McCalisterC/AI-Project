using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsScript : MonoBehaviour
{
    public static SettingsScript instance;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] private int difficulty = 1;

    public void SetDifficultyEasy(){
        difficulty = 0;
    }

    public void SetDifficultyMedium(){
        difficulty = 1;
    }

    public void SetDifficultyHard(){
        difficulty = 2;
    }

    public void SetDifficultyExtreme(){
        difficulty = 3;
    }

    public int GetDifficulty(){
        return difficulty;
    }
}
