using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleButton : MonoBehaviour
{
    public AudioClip clickFX;
    public AudioClip hoverFX;
    public AudioMixerGroup mixer;

    public void OnClick()
    {
        if (clickFX) AudioManager.instance.PlaySFX(clickFX);
    }

    public void OnHover()
    {
        if (hoverFX) AudioManager.instance.PlaySFX(hoverFX);
    }

}
