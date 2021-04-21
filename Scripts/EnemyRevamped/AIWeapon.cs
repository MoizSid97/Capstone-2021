using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    RaycastWeapon currentWeapon;
    Animator anim;
    ParentMeshSocket sockets;
    EnemyRig enemyRig;

    private void Start()
    {
        enemyRig = GetComponentInChildren<EnemyRig>();
        //Access animator component
        //anim = GetComponent<Animator>();
        //Access meshsocket script
        sockets = GetComponent<ParentMeshSocket>();
    }
    //Weapon is now active
    public void ActivateWeapon()
    {
        //Use the bool variable from AIController -> WeaponLayer -> Equip (Parameter)
        //anim.SetBool("Equip", true);
        enemyRig.EquipWeapon();
    }

    public void EquipWeapon(RaycastWeapon weapon)
    {
        //Attach weapon to enemy
        currentWeapon = weapon;
        //Get the parent mesh socket, use the enum property Spine
        sockets.Attach(weapon.transform, ParentMeshSocket.SocketID.Spine);

        sockets.Attach(currentWeapon.transform, ParentMeshSocket.SocketID.RightHand);

        //currentWeapon.transform.SetParent(transform, false);
    }


    //AI will drop his weapon after dying
    public void DropWeapon()
    {
        if(currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    //This animation event is held inside the enemy's idle animation prefab (look for Event dropdown menu)
    public void OnAnimationEvent(string eventName)
    {
        //In the idle animation, go to animation -> events -> there you will see the event this line is accessing/referring to
        if(eventName == "equipWeapon")
        {
            //sockets.Attach(currentWeapon.transform, ParentMeshSocket.SocketID.RightHand);
        }
    }

}
