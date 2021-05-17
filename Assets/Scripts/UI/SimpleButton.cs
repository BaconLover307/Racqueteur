using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : MonoBehaviour
{
    public AudioSource audioSrc;
    public AudioClip clickFX;
    public AudioClip hoverFX;

    public void OnClick()
    {
        if (clickFX) audioSrc.PlayOneShot(clickFX);
    }

    public void OnHover()
    {
        if (hoverFX) audioSrc.PlayOneShot(hoverFX);
    }

}
