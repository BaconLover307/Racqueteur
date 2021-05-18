using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSrc;
    public AudioSource announceAudioSrc;

    public List<Sound> sounds;

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

    public void StopSFX()
    {
        sfxAudioSrc.Stop();
    }

    public void PauseAudio()
    {
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }
        sfxAudioSrc.Pause();
        announceAudioSrc.Pause();
    }

    public void UnPauseAudio()
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
        }
        sfxAudioSrc.UnPause();
        announceAudioSrc.UnPause();
    }

    public void ShutUp()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
        //StopMusic();
        //StopSFX();
        //announceAudioSrc.Stop();
    }

    public void Play(string name)
    {
        Sound s = FindAudioClip(name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = FindAudioClip(name);
        s.source.Stop();
    }

    public Sound FindAudioClip(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null) Debug.LogWarning("Sound: " + name + " not found!");
        return s;
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
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        Play("MenuMusic");
    }

    #endregion

}
