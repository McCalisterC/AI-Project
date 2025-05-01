using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrentUtil : MonoBehaviour
{
    [SerializeField] GameObject utilParent;
    [SerializeField] GameObject util;
    [SerializeField] GameObject utilUI;
    [SerializeField] Sprite utilImage;

    public void SetUtilButton(){
        if(!utilParent.GetComponentInChildren<IUtility>().isOnCooldown){
            utilParent.GetComponentInChildren<IUtility>().DeactivateThis();
            util.SetActive(true);
            utilUI.GetComponent<Image>().sprite = utilImage;
        }
        else{
            //Display Error Message
            Debug.Log("Cannot switch util until it's off of cooldown");
        }
    }
}
