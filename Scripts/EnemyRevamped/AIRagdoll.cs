using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdoll : MonoBehaviour
{
    Rigidbody[] rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //Get rigibody components in children gameobjects
        rb = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        //Foreach rigidbody that has ragdoll physics in the AI's skeleton
        foreach(var rigidbody in rb)
        {
            //Enable kinematic
            rigidbody.isKinematic = true;
        }

        anim.enabled = true;
    }

    public void ActivateRagdoll()
    {
        //Foreach rigidbody that has ragdoll physics in the AI's skeleton
        foreach (var rigidbody in rb)
        {
            //Disable kinematic
            rigidbody.isKinematic = false;
        }

        anim.enabled = false;
    }

    public void ApplyForce(Vector3 force)
    {
        //Add the ability to being pushed for this gameobject 
        var rb = anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.VelocityChange);
    }
}
