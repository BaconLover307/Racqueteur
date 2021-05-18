using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class PauseManager : MonoBehaviour
    {
        public GameObject pauseScreen;
        public AudioClip pauseSFX;
        public AudioClip unPauseSFX;

        private bool _isPaused;
        private float lastPaused = 0f;

        private AudioManager _audioManager;

        public void Awake()
        {
            Time.timeScale = 1f;
            _isPaused = false;
            _audioManager = AudioManager.instance;
        }

        #region public callback

        public void OnPauseClicked()
        {
            if (Time.unscaledTime - lastPaused > 1f)
            {
                if (!_isPaused) PauseGame();
                else ResumeGame();
                lastPaused = Time.unscaledTime;
            }
        }

        public void PauseGame()
        {
            Debug.Log("Paused");
            _isPaused = true;
            _audioManager.PauseAudio();
            _audioManager.PlaySFX(pauseSFX);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }

        public void ResumeGame()
        {
            Debug.Log("UnPaused");
            _isPaused = false;
            _audioManager.PlaySFX(unPauseSFX);
            _audioManager.UnPauseAudio();
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }

        #endregion
    }
}