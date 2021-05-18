using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class PauseManager : MonoBehaviour
    {
        public GameObject pauseScreen;

        private bool _isPaused;
        private float lastPaused = 0f;

        public void Awake()
        {
            Time.timeScale = 1f;
            _isPaused = false;
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
            AudioManager.instance.PauseAudio();
            AudioManager.instance.Play("PauseMenu");
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }

        public void ResumeGame()
        {
            Debug.Log("UnPaused");
            _isPaused = false;
            AudioManager.instance.Play("BackButtonClick");
            AudioManager.instance.UnPauseAudio();
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }

        #endregion
    }
}