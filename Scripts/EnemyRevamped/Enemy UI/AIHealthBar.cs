using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Image FG;
    public Image BG;

    void LateUpdate()
    {
        //If the camera is not facing the enemy
        Vector3 dir = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(dir, Camera.main.transform.forward) <= 0.0f;
        //Hide/disable the UI healthbar
        FG.enabled = !isBehind;
        BG.enabled = !isBehind;

        //Make UI follow AI position above their head
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        FG.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
