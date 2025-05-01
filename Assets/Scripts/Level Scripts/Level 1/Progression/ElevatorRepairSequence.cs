using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorRepairSequence : MonoBehaviour
{
    [SerializeField] GameObject repairEngineer;
    [SerializeField] GameObject elevatorDoors;
    [SerializeField] GameObject taskUI;
    [SerializeField] ElevatorRepairEnemySpawner elevatorRepairEnemySpawner;
    
    public string taskText;
    public int amountNeededToRepair = 100;
    public int repairAmount = 0;
    public int repairRate = 1;

    public bool isRepairing = false;
    public bool repairHasBeenStarted = false;

    // Update is called once per frame
    void Update()
    {
        if(isRepairing && !repairHasBeenStarted && elevatorRepairEnemySpawner.hasTriggered){
            StartRepair();
        }
    }

    public void StartRepair(){
        isRepairing = true;
        repairHasBeenStarted = true;
        taskUI.SetActive(true);
        taskUI.GetComponentInChildren<Slider>().maxValue = amountNeededToRepair;
        taskUI.GetComponentInChildren<TMPro.TMP_Text>().text = taskText;
        StartCoroutine(RepairSequence());
    }

    IEnumerator RepairSequence(){
        if(repairAmount >= amountNeededToRepair){
            EndRepair();
        }
        else{
            yield return new WaitForSeconds(1);
            repairAmount += repairRate;
            taskUI.GetComponentInChildren<Slider>().value = repairAmount;
            StartCoroutine(RepairSequence());
        }
    }

    public void EndRepair(){
        elevatorDoors.SetActive(false);
        repairEngineer.GetComponent<FriendlyElevatorEngineerAiAgent>().stateMachine.ChangeStates(FriendlyElevatorEngineerAiStateId.EndRepair);
        taskUI.SetActive(false);
        elevatorRepairEnemySpawner.StopAllCoroutines();
    }
}
