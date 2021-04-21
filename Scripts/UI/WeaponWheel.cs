using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    [Header("Properties")]
    public float slowMotionTime;
    public GameObject[] uIElements;
    public GameObject weaponWheel;

    [Header("Post Processing")]
    public Volume vol;

    //Colour Adjustment
    private ColorAdjustments ca;
    private float currentContrast = 0;
    private float currentSaturation = 0;
    [Header("Color Adjustment Settings")]
    //public float targetContrast = 100f;
    public float targetSaturation;

    //Vignette
    private Vignette vignetteEffect;
    private float currentIntensity = 0.175f;
    [Header("Vignette Settings")]
    public float targetIntensity = 0.45f;

    //Depth of field
    private DepthOfField dof;
    [Header("Depth of Field Settings")]
    public float focusDistance = 0.1f;

    //Split toning
    private SplitToning splitTone;
    [Header("SplitTone Settings")]
    public float balanceValue;

    //Scripts
    CharacterAiming aimScript;
    RadialMenu radialMenuScript;

    private void Start()
    {
        aimScript = GetComponent<CharacterAiming>();
        radialMenuScript = GetComponent<RadialMenu>();

        weaponWheel.GetComponent<RadialMenu>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //open it up here
            weaponWheel.GetComponent<RadialMenu>().Open();

            aimScript.enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //get the ColorAdjustment probably a good idea to check if it is null
            vol.profile.TryGet(out ca);
            vol.profile.TryGet(out vignetteEffect);
            vol.profile.TryGet(out dof);
            vol.profile.TryGet(out splitTone);

            //access the current values
            //currentContrast = ca.contrast.value;
            currentSaturation = ca.saturation.value;
            currentIntensity = vignetteEffect.intensity.value;

            //set the new values
            //ca.contrast.value = 0;
            ca.saturation.value = targetSaturation;
            vignetteEffect.intensity.value = targetIntensity;

            //depth of field effect
            dof.focusDistance.value = focusDistance;

            //split toning effect
            splitTone.active = true;
            splitTone.balance.value = balanceValue;

            //Time.timeScale = slowMotionTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            //uiElements.SetActive(false);
            for (int i = 0; i < uIElements.Length; i++)
            {
                uIElements[i].SetActive(false);
            }
        }
        else
        {
            Release();
        }
    }

    void Release()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            //close weapon wheel
            weaponWheel.GetComponent<RadialMenu>().Close();

            aimScript.enabled = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //Reset vignette effect (back to normal)
            vol.profile.TryGet(out vignetteEffect);
            currentIntensity = vignetteEffect.intensity.value;
            vignetteEffect.intensity.value = 0.175f;

            //Reset color adjustment
            vol.profile.TryGet(out ca);
            currentSaturation = ca.saturation.value;
            ca.saturation.value = 25;

            //Turn of depth of field
            //vol.profile.TryGet(out dof);
            dof.focusDistance.value = 10;

            //Turn of splittoning
            vol.profile.TryGet(out splitTone);
            splitTone.active = false;

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            //uiElements.SetActive(true);
            for (int i = 0; i < uIElements.Length; i++)
            {
                uIElements[i].SetActive(true);
            }
        }
    }
}
