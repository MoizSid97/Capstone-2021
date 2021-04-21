using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    public float offset = 100f;

    Camera mainCam;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;           
    }

    // Update is called once per frame
    void Update()
    {
        //casts a ray directly onto the screen
        ray.origin = mainCam.transform.position + mainCam.transform.forward * offset;
        ray.direction = mainCam.transform.forward;
        //Physics.Raycast(ray, out hitInfo);
        //transform.position = hitInfo.point;
        if(Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
