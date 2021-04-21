using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCamera : MonoBehaviour
{
    public float cameraSpeed = 2f;
    public int currentIndex = 0;

    public Vector3[] positions;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = positions[currentIndex];

        if (Input.GetKeyUp(KeyCode.E))
        {

            if (currentIndex < positions.Length - 1)
            {
                currentIndex++;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {

            if (currentIndex > 0)
            {
                currentIndex--;
            }
        }

        //Handles moving the camera's position and rotation smoothly
        transform.position = Vector3.Lerp(transform.position, currentPos, cameraSpeed * Time.unscaledDeltaTime);
    }
}
