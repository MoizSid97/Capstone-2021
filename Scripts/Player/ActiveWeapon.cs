using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;
using UnityEngine.Animations;
public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }

    public UnityEngine.Animations.Rigging.Rig handIK;

    public Transform cHTarget;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Transform[] weaponSlots;

    public bool isChanginWeapon;

    public Animator rigController;
    //public Cinemachine.CinemachineFreeLook playerCamera;
    public CharacterAiming characterAiming;

    public UpdateAmmo ammoUI;

    ReloadWeapon reloadScript;
    RaycastWeapon[] equippedWeapons = new RaycastWeapon[2];
    int activeWeaponIndex;
    bool weaponNotActive = false;
    bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();

        //Fixes issues regarding hand IK not aligning with gun animations
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigController.updateMode = AnimatorUpdateMode.Normal;

        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    public bool IsFiring()
    {
        RaycastWeapon currentWeapon = GetActiveWeapon();
        if(!currentWeapon)
        {
            return false;
        }
        return currentWeapon.isFiring;
    }

    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    RaycastWeapon GetWeapon(int index)
    {
        if(index < 0 || index >= equippedWeapons.Length)
        {
            return null;
        }

        return equippedWeapons[index];
    }

    private void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);

        //This line goes to the RigController animator and in the Sprint Layer (2 = 3 third layer)
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("Not Sprinting");

        if(weapon && !weaponNotActive && notSprinting)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }

            //if(Input.GetKeyDown(KeyCode.Mouse0))
            //{
            //    weapon.StartFiring();
            //}

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }

            //if (Input.GetKeyUp(KeyCode.Mouse0))
            //{
            //    weapon.StopFiring();
            //}

            if(Input.GetKeyDown(KeyCode.X))
            {
                //bool isHolstered = rigController.GetBool("holster_weapon");
                //rigController.SetBool("holster_weapon", !isHolstered);

                ToggleActiveWeapon();
            }
        }

        //If user presses 1 key
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Set the primary gun active (plasmarifle)
            SetActiveWeapon(WeaponSlot.Primary);
        }
        //If user presses 2 key
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Set secondary gun active (revolver)
            SetActiveWeapon(WeaponSlot.Secondary);
        }

    }

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.multipleWeapons;
        var weapon = GetWeapon(weaponSlotIndex);
        //If player is already holding a weapon
        if(weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = cHTarget;
        //access recoil stuff
        //weapon.accessRecoilScript.playerCamera = playerCamera;
        weapon.accessRecoilScript.characterAiming = characterAiming;
        //use recoil animations
        weapon.accessRecoilScript.rigController = rigController;
        weapon.transform.parent = weaponSlots[weaponSlotIndex];
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        equippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.multipleWeapons);

        ammoUI.Refresh(weapon.ammoCount);
    }

    void ToggleActiveWeapon()
    {
        bool isHolstered = rigController.GetBool("holster_weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    void SetActiveWeapon(WeaponSlot _multipleWeapons)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)_multipleWeapons;

        //If the activeweapon is already held, it wont repeat the action of equipping if user presses same key
        if(holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int _holsterIndex, int _activateIndex)
    {
        //Line 190 uses the SetInteger from the RigController animator -> Parameter -> weapon_index | 0 = PrimaryWeapon 1 = Secondary Weapon and so forth
        rigController.SetInteger("weapon_index", _activateIndex);

        yield return StartCoroutine(HolsterWeapon(_holsterIndex));
        yield return StartCoroutine(ActivateWeapon(_activateIndex));

        activeWeaponIndex = _activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        isChanginWeapon = true;

        weaponNotActive = true;

        var weapon = GetWeapon(index);
        if(weapon)
        {
            rigController.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }

        isChanginWeapon = false;
    }

    IEnumerator ActivateWeapon(int index)
    {
        isChanginWeapon = true;

        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

            weaponNotActive = false;
        }

        isChanginWeapon = false;

    }

    public void DropWeapon()
    {
        var weapon = GetActiveWeapon();
        if(weapon)
        {
            weapon.transform.SetParent(null);
            weapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            weapon.gameObject.AddComponent<Rigidbody>();
            equippedWeapons[activeWeaponIndex] = null;
        }
    }
}