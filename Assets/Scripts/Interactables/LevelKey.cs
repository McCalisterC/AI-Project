using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelKey : MonoBehaviour, InteractableInterface
{
    public GameObject buttonToActivate;
    [SerializeField] GameObject[] DoorsToOpen;
    [SerializeField] GameObject[] AiToChangeStates;
    [SerializeField] GameObject ElevatorEngineerAi;
    [SerializeField] GameObject elevatorEnemySpawnerTrigger;

    public void Interact(){
        buttonToActivate.GetComponent<ButtonScript>().SetButtonToActive();

        foreach(GameObject obj in DoorsToOpen){
            obj.SetActive(false);
        }

        foreach(GameObject obj in AiToChangeStates){
            obj.GetComponent<FriendlyAiAgent>().stateMachine.ChangeStates(FriendlyAiStateId.FollowPlayer);
        }

        ElevatorEngineerAi.GetComponent<FriendlyElevatorEngineerAiAgent>().stateMachine.ChangeStates(FriendlyElevatorEngineerAiStateId.StartRepair);

        elevatorEnemySpawnerTrigger.SetActive(true);

        this.gameObject.SetActive(false);
    }

    public string Message()
    {
        return "Press F to pick up key";
    }
}
