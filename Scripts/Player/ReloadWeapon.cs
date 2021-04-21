using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    //Editor
    public Transform leftHand;
    public Animator rigController;

    //Audio
    public AudioSource reloadAudio;

    //Private
    private GameObject ammoHand;
    
    //Scripts
    public WeaponAnimationEvent animationEvents;
    public ActiveWeapon activeWeapon;
    public UpdateAmmo ammoUI;

    public bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        animationEvents.AnimationEvent.AddListener(OnAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();

        if(weapon)
        {
            if (Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <= 0)
            {
                isReloading = true;
                rigController.SetTrigger("reload_weapon");
                //reloadAudio.Play();
                weapon.gunFX.Stop();
                weapon.muzzleLight.SetActive(false);
            }

            if (weapon.isFiring)
            {
                ammoUI.Refresh(weapon.ammoCount);
            }

            if (weapon.ammoCount == weapon.clipSize)
            {
                StopReloading();
            }
        }
    }

    void StopReloading()
    {
        rigController.ResetTrigger("reload_weapon");
    }

    void OnAnimationEvent(string eventName)
    {
        Debug.Log(eventName);

        switch(eventName)
        {
            case "detach_ammo":
                DetachAmmo();
                break;
            case "drop_ammo":
                DropAmmo();
                break;
            case "refill_ammo":
                RefillAmmo();
                break;
            case "reattach_ammo":
                ReattachAmmo();
                break;
        }
    }

    //Calling this method instantiates a new prefab of the ammo/magazine
    void DetachAmmo()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        //Create new prefab of magazine
        ammoHand = Instantiate(weapon.magazine, leftHand, true);
        //Hide the current magazine attached to the gun
        weapon.magazine.SetActive(false);
    }

    //Calling this method adds physics to the new magazine prefab
    void DropAmmo()
    {
        GameObject droppedAmmo = Instantiate(ammoHand, ammoHand.transform.position, ammoHand.transform.rotation);
        //Add rigidbody to new magazine prefab
        droppedAmmo.AddComponent<Rigidbody>();
        //Also add a box collider
        droppedAmmo.AddComponent<BoxCollider>();
        //Hide ammo
        ammoHand.SetActive(false);
        //Destroy prefab after 4 seconds
        Destroy(droppedAmmo, 4f);
    }

    void RefillAmmo()
    {
        //Pickup ammo prefab from pocket
        ammoHand.SetActive(true);
    }

    void ReattachAmmo()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        //Unhide the current magazine attached to the gun
        weapon.magazine.SetActive(true);
        Destroy(ammoHand);

        weapon.ammoCount = weapon.clipSize;

        rigController.ResetTrigger("reload_weapon");

        ammoUI.Refresh(weapon.ammoCount);

        isReloading = false;
    }
}
