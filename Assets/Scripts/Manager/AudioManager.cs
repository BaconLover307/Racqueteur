using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainAudioSrc;
    public AudioSource sfxAudioSrc;
    public AudioSource announceAudioSrc;

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

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        if (!mainAudioSrc.isPlaying)
        {
            mainAudioSrc.Play();
        }
    }

    #endregion

}
