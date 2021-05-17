using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainAudioSrc;
    public AudioSource sfxAudioSrc;
    public AudioSource announceAudioSrc;

    [HideInInspector]
    public static AudioManager instance = null;

    #region public callback

    public void PlaySFX(AudioClip audio)
    {
        sfxAudioSrc.PlayOneShot(audio);
    }

    public void PlayAnnounce(AudioClip audio)
    {
        announceAudioSrc.PlayOneShot(audio);
    }

    #endregion

    #region unity callback

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

        if (!mainAudioSrc.isPlaying)
        {
            mainAudioSrc.Play();
        }
    }

    #endregion

}
