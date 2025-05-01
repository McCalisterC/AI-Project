using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleScript : MonoBehaviour
{
    private RectTransform reticle;
    private StarterAssets.StarterAssetsInputs starterAssetsInputs;

    [Range(50f,250f)]
    public float size;

    private void Awake() {
        starterAssetsInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    private void Start(){
        reticle = GetComponent<RectTransform>();
    }
    
    private void Update() {
        GetSize();
    }

    private void GetSize(){
        size = (starterAssetsInputs.currentWeapon.GetWeaponAccuracyPercentage() * starterAssetsInputs.currentWeapon.weaponSpread) + 75;
        reticle.sizeDelta = new Vector2 (size, size);
    }
}
