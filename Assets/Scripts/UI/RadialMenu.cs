using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RadialMenu : MonoBehaviour
{
    [SerializeField]
    StarterAssets.StarterAssetsInputs input;

    [SerializeField]
    WeaponManager weaponManager;

    [SerializeField]
    GameObject EntryPrefab;

    [SerializeField]
    float Radius = 300f;

    [SerializeField] Image background;

    [SerializeField]
    List<WeaponWheelScriptableObject> WeaponInfo;

    [SerializeField]
    List<GameObject> Weapons;
    [SerializeField] Volume blur;
    List<RadialMenuEntry> Entries;

    public bool canOpen = true;
    public bool hasWeapons = false;

    private void Start() {
        Entries = new List<RadialMenuEntry>();
    }

    void AddEntry(string pLabel, Sprite pIcon, RadialMenuEntry.RadialMenuEntryDelegate pCallback, GameObject Weapon){
        GameObject entry = Instantiate(EntryPrefab, transform);

        RadialMenuEntry rme = entry.GetComponent<RadialMenuEntry>();
        rme.SetLabel(pLabel);
        rme.SetIcon(pIcon);
        rme.SetCallback(pCallback);
        rme.SetWeapon(Weapon);

        Entries.Add(rme);
    }

    public void Open(){
        for(int i=0; i < Weapons.Count; i++)
        {
            AddEntry(WeaponInfo[i].label, WeaponInfo[i].icon, SetWeapon, Weapons[i]);
        }
        Rearrange();
    }

    public void Close(){
        for(int i=0; i < Weapons.Count; i++)
        {
            RectTransform rect = Entries[i].GetComponent<RectTransform>();
            GameObject entry = Entries[i].gameObject;

            rect.DOAnchorPos(Vector3.zero, 0.3f).SetEase(Ease.OutQuad).SetUpdate(true).onComplete = 
            delegate(){
                Destroy(entry);
            };
        }

        Entries.Clear();
    }
    
    public void Toggle(){
        if(hasWeapons){
            if(canOpen){
                if(Entries.Count == 0){
                    StopAllCoroutines();
                    blur.weight = 0;
                    StartCoroutine(BlurUp());
                    input.cursorLocked = false;
                    input.cursorInputForLook = false;
                    input.SetCursorState(false);
                    input.pauseInputs = true;
                    input.LookInput(Vector2.zero);
                    background.gameObject.SetActive(true);
                    Open();
                    UnityEngine.Time.timeScale = 0.1f;
                }
                else
                {
                    StopAllCoroutines();
                    blur.weight = 1;
                    StartCoroutine(BlurDown());
                    input.cursorLocked = true;
                    input.cursorInputForLook = true;
                    input.SetCursorState(true);
                    input.pauseInputs = false;
                    UnityEngine.Time.timeScale = 1;
                    background.gameObject.SetActive(false);
                    Close();
                }
            }
        }
    }

    void Rearrange(){
        float radiansOfSeparation = (Mathf.PI * 2) / Entries.Count;
        for(int i = 0; i < Entries.Count; i++){
            float x = Mathf.Sin(radiansOfSeparation * i) * Radius;
            float y = Mathf.Cos(radiansOfSeparation * i) * Radius;
            
            RectTransform rect = Entries[i].GetComponent<RectTransform>();

            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad).SetDelay(.05f * i).SetUpdate(true);
            rect.DOAnchorPos(new Vector3(x,y,0), 0.3f).SetEase(Ease.OutQuad).SetDelay(.05f * i).SetUpdate(true);
        }
    }

    void SetWeapon(RadialMenuEntry pEntry){
        if (weaponManager.GetInputs().currentWeapon.GetIsReloading())
        {
            weaponManager.GetInputs().currentWeapon.StopReload();
        }
        weaponManager.ChangeWeapons(pEntry.Weapon);
    }

    public void SetRHWeapon(GameObject weapon, WeaponWheelScriptableObject label){
        Weapons[Weapons.Count - 1] = weapon;
        WeaponInfo[WeaponInfo.Count - 1] = label;
    }

    IEnumerator BlurUp(){
        if(blur.weight < 1){
            blur.weight += 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(BlurUp());
        }
        else
        {
            blur.weight = 1;
        }
    }

    IEnumerator BlurDown(){
        if(blur.weight > 0){
            blur.weight -= 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(BlurDown());
        }
        else
        {
            blur.weight = 0;
        }
    }
}
