using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private int currentRound;
    public int CurrentRound
    {
        get { return currentRound; }
    }

    private int zombiesToSpawn;
    private int zombiesThisRound;
    public int ZombiesThisRound
    {
        get { return zombiesThisRound; }
        set { zombiesThisRound = value; }
    }
    private int zombiesSpawned;

    private int zombiesKilled;
    public int ZombiesKilled
    {
        get { return zombiesKilled; }
        set { zombiesKilled = value; }
    }

    public GameObject zombie;
    public GameObject[] zombiesSpawnPoints;
    public GameObject ui;

    public void Awake() 
    {
        currentRound = 1;
        zombiesToSpawn = 6;
        zombiesSpawned = 0;
        zombiesKilled = 0;
    }

    public void Start()
    {
        StartRound();
    }

    public void Update()
    {
        if(zombiesThisRound == 0)
        {
            StartCoroutine(EndRound());
            zombiesThisRound = -1;
        }
    }

    public void StartRound()
    {
        zombiesSpawned = 0;
        zombiesToSpawn = 6 * currentRound;
        zombiesThisRound = zombiesToSpawn;
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (zombiesSpawned < zombiesToSpawn)
        {
            if(zombiesSpawned == zombiesToSpawn / 2)
            {
                int spawnPointIndex = Random.Range(0, zombiesSpawnPoints.Length);
                GameObject zomb = Instantiate(zombie, zombiesSpawnPoints[spawnPointIndex].transform.position, zombiesSpawnPoints[spawnPointIndex].transform.rotation);
                zomb.GetComponent<EnemyHealth>().maxHealth = 100 * currentRound;
                zomb.GetComponent<EnemyHealth>().currentHealth = 100 * currentRound;
                zomb.GetComponent<ZombieAiAgent>().isInfectedWithParasite = true;
                zombiesSpawned++;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                int spawnPointIndex = Random.Range(0, zombiesSpawnPoints.Length);
                GameObject zomb = Instantiate(zombie, zombiesSpawnPoints[spawnPointIndex].transform.position, zombiesSpawnPoints[spawnPointIndex].transform.rotation);
                zomb.GetComponent<EnemyHealth>().maxHealth = 100 * currentRound;
                zomb.GetComponent<EnemyHealth>().currentHealth = 100 * currentRound;
                zombiesSpawned++;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public IEnumerator EndRound()
    {
        //Start ui animation transition
        yield return new WaitForSeconds(15f);
        currentRound++;
        ui.GetComponent<HordeModeUI>().UpdateRoundText(currentRound);
        StartRound();
    }
}
