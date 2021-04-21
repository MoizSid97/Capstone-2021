using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //add mouse interaction
using DG.Tweening;
public class WeaponWheelEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleValue = 0.35f;
    public float resetScaleValue = 0.25f;

    [SerializeField]
    Text label;

    [SerializeField]
    RawImage icon;

    RectTransform rect;

    private void Start()
    {
        //access the recttransform of the icon only
        rect = icon.GetComponent<RectTransform>();
    }

    public void SetLabel(string pText)
    {
        label.text = pText;
    }

    public void SetIcon(Texture _icon)
    {
        icon.texture = _icon;
    }

    public Texture GetIcon()
    {
        return (icon.texture);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Clear previous interpolations
        rect.DOComplete();
        //Vector3.one * (insert scale value) to change how big it gets when you hover over icon
        rect.DOScale(Vector3.one * scaleValue, .3f).SetEase(Ease.OutQuad);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Clear previous interpolations
        rect.DOComplete();
        //Reset image scale
        rect.DOScale(Vector3.one * resetScaleValue, .3f).SetEase(Ease.OutQuad);
    }
}
