using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainAudioSrc;
    public AudioSource sfxAudioSrc;
    public AudioSource announceAudioSrc;

    public static AudioManager instance = null;

    #region public function

    public void PlaySFX(AudioClip audio)
    {
        
        sfxAudioSrc.PlayOneShot(audio);
    }

    public void PlayAnnounce(AudioClip audio)
    {
        if (announceAudioSrc.isPlaying)
        {
            announceAudioSrc.Stop();
        }
        announceAudioSrc.PlayOneShot(audio);
    }

    public void StopMusic()
    {
        mainAudioSrc.Stop();
    }

    public void PlayMusic()
    {
        if (!mainAudioSrc.isPlaying)
        {
            mainAudioSrc.Play();
        }
    }

    public void StopSFX()
    {
        sfxAudioSrc.Stop();
    }

    public void PauseAudio()
    {
        mainAudioSrc.Pause();
        sfxAudioSrc.Pause();
        announceAudioSrc.Pause();
    }

    public void UnPauseAudio()
    {
        mainAudioSrc.UnPause();
        sfxAudioSrc.UnPause();
        announceAudioSrc.UnPause();
    }

    public void ShutUp()
    {
        StopMusic();
        StopSFX();
        announceAudioSrc.Stop();
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
    }

    #endregion

}
