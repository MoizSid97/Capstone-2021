using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    //Gameobjects
    public GameObject graphicsOption;
    public GameObject audioOption;
    public GameObject controlsOption;

    //Light
    //public Light sun;

    //SFX
    public AudioSource music;
    public AudioSource[] buttonSfx;

    //Floats
    //public float mouseSensitivity;

    //When game starts, graphics option is first visible, others are disabled
    void Start()
    {
        //Visible
        graphicsOption.SetActive(true); 
        //Hidden
        controlsOption.SetActive(false); 
        //Hidden
        audioOption.SetActive(false); 
    }

    //Clicking on the Graphics Button enables visual options
    public void GraphicsButton()
    {
        //Visible
        graphicsOption.SetActive(true);
        //Hidden
        controlsOption.SetActive(false);
        //Hidden
        audioOption.SetActive(false);
    }

    //Clicking on the Controls Button enables keyboard options
    public void ControlsButton()
    {
        //Hidden
        graphicsOption.SetActive(false);
        //Visible
        controlsOption.SetActive(true);
        //Hidden
        audioOption.SetActive(false);
    }

    //Clicking on the Audio Button enables SFX options
    public void AudioButton()
    {
        //Hidden
        graphicsOption.SetActive(false);
        //Hidden
        controlsOption.SetActive(false);
        //Visible
        audioOption.SetActive(true);
    }

    //For Graphics Button - resolution option 1 is 1440p
    public void Option1()
    {
        Screen.SetResolution(2560, 1440, true);
        Debug.Log("1440p");
    }

    //For Graphics Button - resolution option 2 is 1080p
    public void Option2()
    {
        Screen.SetResolution(1920, 1080, true);
        Debug.Log("1080p");
    }

    //For Graphics Button - resolution option 3 is 900p
    public void Option3()
    {
        Screen.SetResolution(1600, 900, true);
        Debug.Log("900p");
    }

    //For Graphics Button - uses engine's quality settings
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    //For Graphics Button - adjust "brightness" with a slider
    public void SetBrightness(Slider slider)
    {
        //sun.intensity = slider.value;
    }

    //For Graphics Button - toggle screen modes
    public void SetScreenMode(bool isFullScreen)
    {
        Debug.Log("Fullscreen is being toggled");
        Screen.fullScreen = isFullScreen;
    }

    public void MouseControl(Slider senSlider)
    {
        
    }

    //For Audio Button - adjust main menu music
    public void MasterAudio(Slider audioSlider)
    {
        music.volume = audioSlider.value;
    }

    //For Audio Button - adjust effects/buttons audio (using arrays to handle multiple buttons)
    public void MasterSFX(Slider sfxSlider)
    {
        //buttonSfx. = sfxSlider.value;
        foreach(AudioSource eachSource in buttonSfx)
        {
            eachSource.volume = sfxSlider.value;
        }
    }
}
