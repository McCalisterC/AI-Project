using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorRepairEnemySpawner : MonoBehaviour
{
    public bool hasTriggered;
    [SerializeField] GameObject doorToOpen;
    [SerializeField] GameObject[] aiSpawnPoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject elevatorEngineer;
    private List<GameObject> spawnedEnemies;

    private void Awake() {
        spawnedEnemies = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!hasTriggered && other.tag == "Player"){
            hasTriggered = true;
            //Spawn Enemies
            doorToOpen.SetActive(false);
            SpawnEnemies();
            StartCoroutine(CheckEnemyStatus());
        }
    }

    public void SpawnEnemies(){
        foreach(GameObject obj in aiSpawnPoints){
            GameObject tempEnemy = Instantiate(enemyPrefab, obj.transform.position, Quaternion.identity);
            tempEnemy.GetComponent<AiAgent>().initialState = AiStateId.ChasePlayer;
            spawnedEnemies.Add(tempEnemy);
            tempEnemy.GetComponent<AiAgent>().targetObject = elevatorEngineer;
            if(tempEnemy.GetComponent<AiAgent>().stateMachine == null)
                tempEnemy.GetComponent<AiAgent>().MakeStateMachine();
        }
    }

    IEnumerator CheckEnemyStatus(){
        List<GameObject> tempList = new List<GameObject>();

        foreach(GameObject obj in spawnedEnemies){
            Debug.Log("Checked Enemy Death Status");
            if(obj.GetComponent<AiAgent>().HasDied()){
                GameObject tempEnemy = Instantiate(enemyPrefab, aiSpawnPoints[Random.Range(0,aiSpawnPoints.Length)].transform.position, Quaternion.identity);
                tempEnemy.GetComponent<AiAgent>().initialState = AiStateId.ChasePlayer;
                tempList.Add(tempEnemy);
                tempEnemy.GetComponent<AiAgent>().targetObject = elevatorEngineer;
                if(tempEnemy.GetComponent<AiAgent>().stateMachine == null)
                    tempEnemy.GetComponent<AiAgent>().MakeStateMachine();
            }
            else{
                tempList.Add(obj);
            }
        }

        spawnedEnemies = tempList;

        yield return new WaitForSeconds(10f);
        StartCoroutine(CheckEnemyStatus());
    }
}
