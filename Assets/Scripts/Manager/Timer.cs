using System;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class Timer : MonoBehaviour
    {
        public float timeRemaining = 3 * 60;
        public string monoSpacingSize = "30";
        [SerializeField] private bool timerStarted;

        private TMP_Text _timerText;
        public Action OnTimerEnd;
        public Action OnTimerNotification;
        public Action OnTimerLastCountdown;

        #region private function

        private void DisplayTimer()
        {
            var minutes = Math.Floor(timeRemaining / 60);
            var seconds = Math.Floor(timeRemaining % 60);
            _timerText.text = $"<mspace=mspace={monoSpacingSize}>{minutes:00}:{seconds:00}</mspace>";
        }

        #endregion

        #region public function

        public void StartTimer()
        {
            timerStarted = true;
        }

        public void StopTimer()
        {
            timerStarted = false;
        }

        #endregion

        #region unity callback

        private void Awake()
        {
            _timerText = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            DisplayTimer();
        }

        private void Update()
        {
            if (!timerStarted) return;

            if (timeRemaining > 0) timeRemaining -= Time.deltaTime;

            timeRemaining = Math.Max(timeRemaining, 0);

            DisplayTimer();

            if (timeRemaining == 0)
            {
                timerStarted = false;
                OnTimerEnd?.Invoke();
            }
            else if ((Math.Floor(timeRemaining) == 10) || (Math.Floor(timeRemaining) == 30) || (Math.Floor(timeRemaining) == 60))
            {
                OnTimerNotification?.Invoke();
            }
            else if ((Math.Floor(timeRemaining) == 5))
            {
                OnTimerLastCountdown?.Invoke();
            }
        }

        #endregion
    }
}