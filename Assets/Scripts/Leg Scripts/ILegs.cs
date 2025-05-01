using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILegs
{
    public string name{get; set;}
    public void ModifyMovement();
    public void DisableLegs();
}
