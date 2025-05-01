using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentHull : MonoBehaviour
{
    [SerializeField] GameObject hull;
    PlayerStats playerStats;

    private void Start() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }
    public void SetHullButton(){
        playerStats.SetCurrentHull(hull);
    }
}
