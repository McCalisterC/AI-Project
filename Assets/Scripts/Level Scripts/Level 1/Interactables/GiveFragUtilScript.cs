using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveFragUtilScript : MonoBehaviour, InteractableInterface
{
    [SerializeField] GameObject fragObject;
    [SerializeField] SetCurrentUtil setCurrentUtil;

    public void Interact(){
        setCurrentUtil.SetUtilButton();
        this.tag = "Untagged";
    }

    public string Message()
    {
        return "Press F to pick up frag grenade";
    }
}
