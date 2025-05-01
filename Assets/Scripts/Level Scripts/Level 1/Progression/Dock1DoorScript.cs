using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock1DoorScript : MonoBehaviour
{
    [SerializeField] List<GameObject> requiredKills;
    [SerializeField] private int frequency = 30;
    [SerializeField] float interval;
    [SerializeField] float timer;

    void Start()
    {
        interval = 1.0f / frequency;
    }

    void Update(){
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer += interval;
            CheckDeaths();
        }
    }
    
    void CheckDeaths(){
        if(requiredKills.Count == 0){
            Destroy(this.gameObject);
        }
        else{
            foreach (var obj in requiredKills){
                if(obj.GetComponent<AiAgent>().stateMachine.currentState == AiStateId.Death){
                    requiredKills.Remove(obj);
                }
            }
        }
        
    }
}
