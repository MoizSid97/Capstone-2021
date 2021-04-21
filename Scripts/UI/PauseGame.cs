using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //Audio
    [Header("Pause Audio")]
    public AudioSource pauseFX;
    public AudioSource resumeFX;

    //Post processing
    [Header("Vignette Post Processing Settings")]
    public Volume vol;
    private ChromaticAberration ca;
    private float currentIntensity = 0;
    public float targetIntensity = 0.25f;

    [Header("Enable/Disable GameObjects")]
    public GameObject hudCanvas;
    public GameObject pauseCanvas;
    public GameObject mainCamera;
    public GameObject pauseCamera;
    public GameObject playerModel;
    public GameObject eqButtons;
    public GameObject[] enemies;

    [Header("Check Pause")]
    public bool isPaused;

    //Scripts
    private ActiveWeapon activeWeapon;
    private CharacterAiming aimScript;
    public PauseCamera pause;

    private void Start()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        aimScript = GetComponent<CharacterAiming>();

        pause.GetComponent<PauseCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(isPaused)
            {

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                ResumeGame();
            }
            else
            {

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                GameIsPaused();
            }
        }
    }

    void GameIsPaused()
    {
        pauseFX.Play();

        Time.timeScale = 0.0f;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
        }

        activeWeapon.enabled = false;
        aimScript.enabled = false;

        hudCanvas.SetActive(false);
        pauseCanvas.SetActive(true);

        mainCamera.SetActive(false);
        pauseCamera.SetActive(true);

        playerModel.SetActive(true);

        eqButtons.SetActive(true);

        //get the ColorAdjustment probably a good idea to check if it is null
        vol.profile.TryGet(out ca);
        //access the current values
        currentIntensity = ca.intensity.value;
        //set the new values
        ca.intensity.value = targetIntensity;
    }

    void ResumeGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;

        activeWeapon.enabled = true;
        aimScript.enabled = true;

        hudCanvas.SetActive(true);
        pauseCanvas.SetActive(false);

        mainCamera.SetActive(true);
        pauseCamera.SetActive(false);

        playerModel.SetActive(false);

        //get the ColorAdjustment probably a good idea to check if it is null
        vol.profile.TryGet(out ca);
        //access the current values
        currentIntensity = ca.intensity.value;
        //set the new values
        ca.intensity.value = 0;
    }

    public void ResumeButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        resumeFX.Play();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }

        Time.timeScale = 1.0f;
        isPaused = false;

        activeWeapon.enabled = true;
        aimScript.enabled = true;

        hudCanvas.SetActive(true);
        pauseCanvas.SetActive(false);

        mainCamera.SetActive(true);
        pauseCamera.SetActive(false);

        playerModel.SetActive(false);

        eqButtons.SetActive(false);

        //get the ColorAdjustment probably a good idea to check if it is null
        vol.profile.TryGet(out ca);
        //access the current values
        currentIntensity = ca.intensity.value;
        //set the new values
        ca.intensity.value = 0;
    }

    public void SettingsButton()
    {
        pause.currentIndex = 1;
    }

    public void ExitButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
}
