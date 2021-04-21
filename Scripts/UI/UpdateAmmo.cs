using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAmmo : MonoBehaviour
{
    public Text ammoText;

    public void Refresh(int ammoCount)
    {
        ammoText.text = ammoCount.ToString();
    }
}
