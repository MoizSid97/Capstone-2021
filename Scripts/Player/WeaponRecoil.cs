using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    //[HideInInspector] public Cinemachine.CinemachineFreeLook playerCamera;
    [HideInInspector] public CharacterAiming characterAiming;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;

    //Public Vector
    public Vector2[] recoilPattern;
    
    //Public Floats
    public float duration;
    public float recoilModifier = 1f;

    //Private Floats
    float time;
    float verticalRecoil;
    float horizontalRecoil;
    
    //Private Integers
    int index;
    int recoilLayerIndex = -1;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        //if(rigController)
        //{
        //    recoilLayerIndex = rigController.GetLayerIndex("Recoil Layer");
        //}
    }

    public void Reset()
    {
        index = 0;
    }

    int NextIndex(int index)
    {
        return (index + 1) % recoilPattern.Length;
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;

        //Add camera shake via impusle from cinemachine
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        //Use horizontal vector array
        horizontalRecoil = recoilPattern[index].x;
        //Use vertical vector array
        verticalRecoil = recoilPattern[index].y;

        //Go to next index in the array
        index = NextIndex(index);

        //Use recoil animation
        rigController.Play("anim_recoil_" + weaponName, 1, 0.0f);
        //rigController?.Play("anim_recoil_" + weaponName, recoilLayerIndex, 0.0f);
        //rigController?.Play("weapon_" + weaponName + "_recoil", recoilLayerIndex, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            //Modify Y value for vertical recoil
            characterAiming.yAxis.Value -= (((verticalRecoil / 10) * Time.deltaTime) / duration) * recoilModifier;
            //Modify X value for horizontal recoil
            characterAiming.xAxis.Value -= (((horizontalRecoil / 10) * Time.deltaTime) / duration) * recoilModifier;

            time -= Time.deltaTime;
        }
        
    }
}
