using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRig : MonoBehaviour
{
    Animator enemyRigController;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigController = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipWeapon()
    {
        //Use the bool variable from AIRigController -> WeaponLayer -> EquipWeapon (Parameter)
        enemyRigController.SetBool("EquipWeapon", true);
    }
}
