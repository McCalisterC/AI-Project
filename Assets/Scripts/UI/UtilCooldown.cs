using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UtilCooldown : MonoBehaviour
{
    float cooldownTime;
    float currentBar;
    bool coolingDown = false;
    public void StartCooldownAnim(float cooldownTime){
        this.cooldownTime = cooldownTime;
        this.GetComponent<Image>().fillAmount = 0;
        this.GetComponent<Image>().color = Color.red;
        coolingDown = true;
    }
    private void Update()
    {
        if (coolingDown) 
        {
            this.GetComponent<Image>().fillAmount += 1.0f / cooldownTime * Time.deltaTime;
            if(this.GetComponent<Image>().fillAmount >= 1)
            {
                this.GetComponent<Image>().fillAmount = 1;
                this.GetComponent<Image>().color = Color.green;
                coolingDown = false;
            }
        }
    }
}
