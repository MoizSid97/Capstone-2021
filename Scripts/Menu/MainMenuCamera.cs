using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCamera : MonoBehaviour
{
    public Vector3[] positions;
    public Quaternion[] newRotation;
    public float cameraSpeed = 2f;
    private int currentIndex = 0;
    private int rotCurrentIndex = 0;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = positions[currentIndex];
        Quaternion currentRot = newRotation[rotCurrentIndex];

        if(Input.GetKeyUp(KeyCode.Return))
        {
            //if(currentIndex < positions.Length - 1)
            //{
            //    currentIndex++;
            //}

            //if(rotCurrentIndex < newRotation.Length - 1)
            //{
            //    rotCurrentIndex++;
            //}

            currentIndex = 1;
            rotCurrentIndex = 1;
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            //if(currentIndex > 0)
            //{
            //    currentIndex--;
            //}

            //if(rotCurrentIndex > 0)
            //{
            //    rotCurrentIndex--;
            //}

            currentIndex = 0;
            rotCurrentIndex = 0;
        }

        //Handles moving the camera's position and rotation smoothly
        transform.position = Vector3.Lerp(transform.position, currentPos, cameraSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentRot, cameraSpeed * Time.deltaTime);
    }

    //Load next level
    public void PlayButton(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }

    //Upon clicking settings button, go to listed index
    public void SettingsButton()
    {
        currentIndex = 2;
        rotCurrentIndex = 2;
    }

    public void MarketButton()
    {
        currentIndex = 3;
        rotCurrentIndex = 3;
    }

    //Quit game
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game Has Quit");
    }

    public void ExitSettingsMenu()
    {
        currentIndex = 1;
        rotCurrentIndex = 1;
    }
}
