using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintLegsScript : MonoBehaviour, ILegs
{
    public string _name;
    new string name{get => _name; set => _name = value;}
    public void ModifyMovement(){
        //Not necessary
    }

    public void DisableLegs(){
        this.gameObject.SetActive(false);
    }
}
