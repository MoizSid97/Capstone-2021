using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Transform bone; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = targetTransform.position;
        AimAtTarget(bone, targetPos);
    }

    void AimAtTarget(Transform bone, Vector3 targetPos)
    {
        Vector3 aimDir = aimTransform.forward;
        Vector3 targetDir = targetPos - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDir, targetDir);
        bone.rotation = aimTowards * bone.rotation;
    }
}
