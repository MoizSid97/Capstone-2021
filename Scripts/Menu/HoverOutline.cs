using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{

    public Outline script;

    private void OnMouseOver()
    {
        script.enabled = true;
    }

    private void OnMouseExit()
    {
        script.enabled = false;
    }
}
