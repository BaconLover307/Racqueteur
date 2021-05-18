using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    public static AudioManager instance = null;

    #region public function

    public void PauseAudio()
    {
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }
    }

    public void UnPauseAudio()
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
        }
    }

    public void ShutUp()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void Play(string name)
    {
        Sound s = FindAudioClip(name);
        if (s != null) s.source.Play();
    }
    public void PlayOneShot(string name)
    {
        Sound s = FindAudioClip(name);
        if (s != null) s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {
        Sound s = FindAudioClip(name);
        if (s != null) s.source.Stop();
    }

    public Sound FindAudioClip(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
        }
        return s;
    }

    public void PlayMusic()
    {
        Sound s = FindAudioClip("MenuMusic");
        if (s != null && !s.source.isPlaying)
        {
            s.source.Play();
        }
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
            s.source.outputAudioMixerGroup = s.mixer;
        }

        PlayMusic();
    }

    #endregion

}
