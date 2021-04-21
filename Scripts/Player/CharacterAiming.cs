using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float aimDuration = 0.3f;

    public Transform cameraLookAt;

    public bool isAiming;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    //Post processing
    public Volume vol;
    private Vignette vignetteEffect;
    private float currentIntensity = 0.175f;
    [Header("Vignette Post Processing Settings")]
    public float targetIntensity = 0.25f;

    ActiveWeapon accessActiveWeapon;

    Camera mainCam;
    Animator playerAnim;

    int isAimingParameter = Animator.StringToHash("IsAiming");

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerAnim = GetComponent<Animator>();
        accessActiveWeapon = GetComponent<ActiveWeapon>();

    }

    private void Update()
    {
        //If user presses right mouse button
        isAiming = Input.GetMouseButton(1);
        //Do the zoom in effect
        playerAnim.SetBool(isAimingParameter, isAiming);

        if(Input.GetMouseButton(1))
        {
            //get the ColorAdjustment probably a good idea to check if it is null
            vol.profile.TryGet(out vignetteEffect);
            //access the current values
            currentIntensity = vignetteEffect.intensity.value;
            //set the new values
            vignetteEffect.intensity.value = targetIntensity;
        }

        if(Input.GetMouseButtonUp(1))
        {
            //get the ColorAdjustment probably a good idea to check if it is null
            vol.profile.TryGet(out vignetteEffect);
            //access the current values
            currentIntensity = vignetteEffect.intensity.value;
            //set the new values
            vignetteEffect.intensity.value = 0.175f;
        }

        //If player is holding a gun
        var weapon = accessActiveWeapon.GetActiveWeapon();
        if(weapon)
        {
            weapon.accessRecoilScript.recoilModifier = isAiming ? 0.3f : 1.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        //rotate camera on the y axis
        float yawCamera = mainCam.transform.rotation.eulerAngles.y;
        //blend players rotation towards cameras
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
