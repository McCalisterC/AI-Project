using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RadialMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void RadialMenuEntryDelegate(RadialMenuEntry pEntry);

    [SerializeField]
    TextMeshProUGUI Label;

    [SerializeField]
    Image Icon;

    [SerializeField]
    public GameObject Weapon;

    RectTransform Rect;
    RadialMenuEntryDelegate Callback;

    private void Start() {
        Rect = GetComponent<RectTransform>();
    }

    public void SetLabel(string pText){
        Label.text = pText;
    }

    public void SetIcon(Sprite pIcon){
        Icon.sprite = pIcon;
    }

    public Sprite GetIcon(){
        return Icon.sprite;
    }

    public void SetCallback(RadialMenuEntryDelegate pCallback){
        Callback = pCallback;
    }

    public void SetWeapon(GameObject weapon){
        Weapon = weapon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Callback?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Rect.DOComplete();
        Rect.DOScale(Vector3.one * 1.5f, .3f).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Rect.DOComplete();
        Rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
}
