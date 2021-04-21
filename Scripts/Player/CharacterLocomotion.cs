using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CharacterLocomotion : MonoBehaviour
{
    //Floats
    [Header("Player Settings")]
    public float health = 100f;
    public float walkSpeed;
    [Header("Player Jump Settings")]
    public float jumpHeight;
    public float jumpDamp;
    public float airControl;
    [Header("Physics Settings")]
    public float stepDown;
    public float gravity;
    public float pushPower;

    //Editor
    [Header("Editor Tools")]
    public Animator rigController;
    VolumeProfile vol;

    //Audio
    [Header("Audio Components")]
    public AudioSource coinSFX;
    public AudioSource weaponPickUpSFX;

    //Scripts
    ActiveWeapon accessActiveWeapon;
    ReloadWeapon accessReload;
    CharacterAiming accessCharacterAiming;
    AIRagdoll ragdoll;
    PauseGame pauseGameScript;

    //Private Editor
    Animator playerAnim;
    CharacterController playerCC;

    //Private Vectors
    Vector2 input;
    Vector3 rootMotion;
    Vector3 velocity;

    //Private Bools
    bool isJumping;

    //Private Int
    int isSprintingParameter = Animator.StringToHash("IsSprinting");
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerCC = GetComponent<CharacterController>();
        accessActiveWeapon = GetComponent<ActiveWeapon>();
        accessReload = GetComponent<ReloadWeapon>();
        accessCharacterAiming = GetComponent<CharacterAiming>();
        ragdoll = GetComponent<AIRagdoll>();
        pauseGameScript = GetComponent<PauseGame>();

        vol = FindObjectOfType<Volume>().profile;
    }

    // Update is called once per frame
    void Update()
    {
        //Move horizontal or vertical
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        //Use the float parameters from Animator component
        playerAnim.SetFloat("InputX", input.x);
        playerAnim.SetFloat("InputY", input.y);

        //Calling method that handles sprinting
        UpdateSprint();

        //Pressing spacebar makes the player jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(health <= 0)
        {
            ChromaticAberration ca;
            ColorAdjustments color;
            if (vol.TryGet(out ca))
            {
                float percent = 1.0f;
                ca.intensity.value = percent * 0.5f;
            }
            if(vol.TryGet(out color))
            {
                color.saturation.value = -100;
            }
            HandlePlayerHealth();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    bool Sprint()
    {
        //Pressing left shit will put us in sprint/run mode
        bool startRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        //***THESE BOOLEANS WILL MAKE THE PLAYER STOP RUNNING TO CHECK IF WE ARE DOING CERTAIN COMMANDS***//
        //Check if we are shooting
        bool startFiring = accessActiveWeapon.IsFiring();
        //Check if we are reloading
        bool isReloading = accessReload.isReloading;
        //Check if we are changing guns
        bool isChangingWeapon = accessActiveWeapon.isChanginWeapon;
        //Check if we are aiming our gun
        bool isAiming = accessCharacterAiming.isAiming;

        return startRunning && !startFiring &&!isReloading && !isChangingWeapon &&!isAiming;
    }

    //Handles the player running
    void UpdateSprint()
    {
        bool isSprinting = Sprint();
        playerAnim.SetBool(isSprintingParameter, isSprinting);
        rigController.SetBool(isSprintingParameter, isSprinting);
    }



    private void OnAnimatorMove()
    {
        rootMotion += playerAnim.deltaPosition;
    }

    private void FixedUpdate()
    {
        //In air state
        if(isJumping)
        {
            InAirState();
        }
        //Player is grounded
        else
        {
            GroundedState();
        }

    }

    void GroundedState()
    {
        //Fixing the jittery ground movement | Vector3.down * stepDown fixes floating in air after climbing slopes
        Vector3 stepForwardAmount = rootMotion * walkSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;

        playerCC.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        //Player will no longer snap to ground when falling from an object
        if (!playerCC.isGrounded)
        {
            //isJumping = true;
            //velocity = playerAnim.velocity * jumpDamp;
            //velocity.y = 0;

            SetInAir(0);
        }
    }

    void InAirState()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += AirControl();
        playerCC.Move(displacement);
        isJumping = !playerCC.isGrounded;
        //This fixes when player jumps and he moves, he will no longer teleport to next position
        rootMotion = Vector3.zero;

        playerAnim.SetBool("IsJumping", isJumping);
    }

    Vector3 AirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }

    void Jump()
    {
        if(!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = playerAnim.velocity * jumpDamp * walkSpeed;
        velocity.y = jumpVelocity;
        playerAnim.SetBool("IsJumping", true);
    }

    //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnControllerColliderHit.html
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hazard"))
        {
            health -= 100;
        }

        if(other.CompareTag("Coin"))
        {
            coinSFX.Play();
        }

        if (other.CompareTag("WeaponPickUp"))
        {
            weaponPickUpSFX.Play();
        }

        //if(other.CompareTag("ControlPanel"))
        //{
        //    if(Input.GetKeyDown(KeyCode.E))
        //    {
        //        SceneManager.LoadScene(2);
        //    }
        //}
    }

    public void HandlePlayerHealth()
    {
        ragdoll.ActivateRagdoll();
        accessActiveWeapon.DropWeapon();
        accessCharacterAiming.enabled = false;
        pauseGameScript.enabled = false;
        //Destroy(gameObject, 10f);
    }
}
