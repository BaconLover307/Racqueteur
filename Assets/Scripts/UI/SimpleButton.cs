using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleButton : MonoBehaviour
{
    public AudioClip clickFX;
    public AudioClip hoverFX;
    public AudioMixerGroup mixer;

    private AudioSource audioSrc;

    private void Awake()
    {
        AudioSource[] sources = GameObject.FindGameObjectWithTag("Audio").GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            if (source.outputAudioMixerGroup.Equals(mixer))
            {
                audioSrc = source;
                break;
            }
        }
    }

    public void OnClick()
    {
        if (clickFX && audioSrc) audioSrc.PlayOneShot(clickFX);
    }

    public void OnHover()
    {
        if (hoverFX && audioSrc) audioSrc.PlayOneShot(hoverFX);
    }

}
