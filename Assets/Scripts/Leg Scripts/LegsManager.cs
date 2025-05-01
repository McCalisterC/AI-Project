using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsManager : MonoBehaviour
{
    private void Awake() {
        this.GetComponentInChildren<ILegs>().ModifyMovement();
    }
}
