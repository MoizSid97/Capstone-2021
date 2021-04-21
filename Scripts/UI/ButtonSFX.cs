using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource myFX;
    public AudioClip hoverFX;
    public AudioClip clickFX;

    //When mouse hovers over button, play this audio
    public void Hover()
    {
        myFX.PlayOneShot(hoverFX);
    }

    //When mouse clicks on button, play this audio
    public void Clicked()
    {
        myFX.PlayOneShot(clickFX);
    }
     

}
