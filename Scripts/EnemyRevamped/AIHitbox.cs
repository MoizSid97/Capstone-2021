using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitbox : MonoBehaviour
{
    public AILocomotion healthValue;

    public void OnRayCastHit(RaycastWeapon weapon, Vector3 dir)
    {
        //Take damage provided by the RaycastWeapon script set value
        healthValue.TakeDamage(weapon.bulletDMG, dir);
    }
}
