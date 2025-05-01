using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLegsScript : MonoBehaviour
{
    [SerializeField] GameObject legsParent;
    [SerializeField] GameObject legsObject;
    PlayerStats playerStats;

    private void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void SetLegsButton(){
        legsParent.GetComponentInChildren<ILegs>().DisableLegs();
        legsObject.SetActive(true);
        legsObject.GetComponent<ILegs>().ModifyMovement();
        playerStats.SetCurrentLegs(legsObject);
    }
}
