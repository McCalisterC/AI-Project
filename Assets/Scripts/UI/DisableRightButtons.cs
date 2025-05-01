using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRightButtons : MonoBehaviour
{
    public void DisableRightButton(){
        Canvas[] list = this.GetComponentsInChildren<Canvas>();
        for (int i = 0; i < list.Length; i++)
        {
            list[i].gameObject.SetActive(false);
        }
    }
}
