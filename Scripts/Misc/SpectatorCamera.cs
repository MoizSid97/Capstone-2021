using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpectatorCamera : MonoBehaviourPun
{
    public float movementSpeed = 1;

    public float speedH = 2;
    public float speedV = 2;

    private float yaw = 0;
    private float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Locks mouse to centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        //Mouse not visible
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(base.photonView.IsMine)
        {
            //h = horizontal, use A & D
            float h = Input.GetAxis("Horizontal") * movementSpeed;
            //v = vertical, use W & S
            float v = Input.GetAxis("Vertical") * movementSpeed;

            //Updates rotation for camera based on mouse movement
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
        //Sets camera rotation to pitch and yaw variables
        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        //Updating camera's position
        transform.position += transform.TransformDirection(Vector3.forward) * v + transform.TransformDirection(Vector3.right) * h;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        }



    }
}
