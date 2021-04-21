using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        //If player picks up weapon
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if(activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            activeWeapon.Equip(newWeapon);
            //Destroys weapon gameobject after being picked up by player
            Destroy(gameObject);
        }

        //If Enemy AI picks up weapon
        //AIHitbox hitBox = other.gameObject.GetComponent<AIHitbox>();
        //if (hitBox)
        //{
        //    AIWeapon weapons = hitBox.healthValue.GetComponent<AIWeapon>();
        //    if(weapons != null)
        //    {
        //        RaycastWeapon newWeapon = Instantiate(weaponPrefab);
        //        weapons.EquipWeapon(newWeapon);
        //        Destroy(gameObject);
        //    }
        //}

        AIWeapon aiWeapon = other.gameObject.GetComponent<AIWeapon>();
        if(aiWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            aiWeapon.EquipWeapon(newWeapon);
            Destroy(gameObject);
        }    
    }
}
