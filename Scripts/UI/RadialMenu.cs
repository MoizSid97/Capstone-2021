using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RadialMenu : MonoBehaviour
{
    //https://www.youtube.com/watch?v=tdkdRguH_dE&ab_channel=PabloPoon

    [SerializeField]
    GameObject entryPrefab;
    
    [SerializeField]
    float radius = 300f;

    [SerializeField]
    List<Texture> icons;

    List<WeaponWheelEntry> entries;
    
    // Start is called before the first frame update
    void Start()
    {
        //Access the weaponwheelentry prefabs/script
        entries = new List<WeaponWheelEntry>();
    }

    private void Update()
    {
    }

    void AddEntry(string pLabel, Texture _icon)
    {
        //Spawning the prefab
        GameObject entry = Instantiate(entryPrefab, transform);

        //Access this script
        WeaponWheelEntry wwe = entry.GetComponent<WeaponWheelEntry>();

        //Setting text label
        wwe.SetLabel(pLabel);
        //Setting icons
        wwe.SetIcon(_icon);

        //Add entries into the weapon wheel
        entries.Add(wwe);
    }

    //Open the wheel
    public void Open()
    {
         for (int i = 0; i < 4; i++)
         {
            AddEntry("Weapon" + i.ToString(), icons[i]);
         }

         Rearrange();
    }

    //Close the wheel
    public void Close()
    {
        for (int i = 0; i < 4; i++)
        {
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            GameObject entry = entries[i].gameObject;
            rect.DOAnchorPos(Vector3.zero, 0.1f).SetEase(Ease.OutQuad).onComplete = delegate () { Destroy(entry); };
        }

        entries.Clear();
    }

    public void Toggle()
    {
        if (entries.Count == 0)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    //This function arranges them in a circle shape
    void Rearrange()
    {
        float radiansOfSeperation = (Mathf.PI * 2) / entries.Count;

        for (int i = 0; i < entries.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeperation * i) * radius;
            float y = Mathf.Cos(radiansOfSeperation * i) * radius;

            //entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);

            RectTransform rect = entries[i].GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutQuad).SetDelay(.05f * i);
            rect.DOAnchorPos(new Vector3(x, y, 0), 0.6f).SetEase(Ease.OutQuad).SetDelay(.05f * i);
        }
    }
}
