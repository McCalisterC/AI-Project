using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveUI : MonoBehaviour
{
    
    [SerializeField] GameObject[] uiToBeRemoved;

    public void RemoveUIFuction(){
        if(uiToBeRemoved.Length > 0){
            foreach(GameObject obj in uiToBeRemoved){
                obj.SetActive(false);
            }
        }
    }

    public void ReactivateUI(){
        if(uiToBeRemoved.Length > 0){
            foreach(GameObject obj in uiToBeRemoved){
                obj.SetActive(true);
            }
        }
    }
}
