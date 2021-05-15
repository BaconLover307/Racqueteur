using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class PauseManager : MonoBehaviour
    {
        public GameObject pauseScreen;

        private bool _isPaused;

        public void Awake()
        {
            Time.timeScale = 1f;
        }

        #region public callback

        public void OnPauseClicked()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
                PauseGame();
            else
                ResumeGame();
        }

        public void PauseGame()
        {
            _isPaused = false;
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }

        public void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }

        #endregion
    }
}