using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Manager.Options
{
    public class AudioOptions : MonoBehaviour
    {
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;
        public Slider annSlider;
        public AudioMixer mainMix;

        private void Start()
        {
            // Set sound volume
            masterSlider.value = PlayerPrefs.GetFloat("masterVol", 0.5f);
            musicSlider.value = PlayerPrefs.GetFloat("musicVol", 0.35f);
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.75f);
            annSlider.value = PlayerPrefs.GetFloat("annVol", 0.6f);
        }

        public void OnMasterVolumeChanged(float value)
        {
            mainMix.SetFloat("masterVol", Mathf.Log10(value) * 20);

            PlayerPrefs.SetFloat("masterVol", value);

            PlayerPrefs.Save();
        }

        public void OnMusicVolumeChanged(float value)
        {
            mainMix.SetFloat("musicVol", Mathf.Log10(value) * 20);

            PlayerPrefs.SetFloat("musicVol", value);

            PlayerPrefs.Save();
        }

        public void OnSFXVolumeChanged(float value)
        {
            mainMix.SetFloat("sfxVol", Mathf.Log10(value) * 20);

            PlayerPrefs.SetFloat("sfxVol", value);

            PlayerPrefs.Save();
        }

        public void OnAnnVolumeChanged(float value)
        {
            mainMix.SetFloat("annVol", Mathf.Log10(value) * 20);

            PlayerPrefs.SetFloat("annVol", value);

            PlayerPrefs.Save();
        }
    }
}