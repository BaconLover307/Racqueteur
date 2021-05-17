using System;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class Timer : MonoBehaviour
    {
        public float timeRemaining = 3 * 60;
        public string monoSpacingSize = "30";
        public bool displayTimer = false;
        [SerializeField] private bool timerStarted;

        public Action OnTimerEnd;
        public Action OnTimerNotification;
        public Action OnTimerLastCountdown;

        private TMP_Text _timerText;
        private bool hasInvoked = false;

        #region private function

        private void DisplayTimer(float time = -1)
        {
            if (time == -1)
            {
                time = timeRemaining;
            }
            var minutes = Math.Floor(time / 60);
            var seconds = Math.Floor(time % 60);
            _timerText.text = $"<mspace=mspace={monoSpacingSize}>{minutes:0}:{seconds:00}</mspace>";
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
            DisplayTimer(timeRemaining - 4);
            StartTimer();
        }

        private void SetHasInvokeFalse()
        {
            hasInvoked = false;
        }

        private void TriggerInvoke()
        {
            hasInvoked = true;
            Invoke(nameof(SetHasInvokeFalse), 1.5f);
        }

        private void Update()
        {
            if (!timerStarted) return;

            if (timeRemaining > 0) timeRemaining -= Time.deltaTime;

            timeRemaining = Math.Max(timeRemaining, 0);

            if (displayTimer) DisplayTimer(-1);

            if (timeRemaining <= 0 && !hasInvoked)
            {
                displayTimer = false;
                timerStarted = false;
                OnTimerEnd?.Invoke();
                TriggerInvoke();
            }
            else if ((Math.Floor(timeRemaining) == 30) || (Math.Floor(timeRemaining) == 60) && !hasInvoked)
            {
                OnTimerNotification?.Invoke();
                TriggerInvoke();
            }
            else if ((Math.Floor(timeRemaining) == 10) && !hasInvoked)
            {
                OnTimerLastCountdown?.Invoke();
                TriggerInvoke();
            }
        }

        #endregion
    }
}