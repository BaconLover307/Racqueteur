using System;
using TMPro;
using UnityEngine;

namespace Manager.Options
{
    public class ErrorPopup : MonoBehaviour
    {
        public static ErrorPopup Instance;
        
        public GameObject popup;
        public TMP_Text errorText;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            popup.SetActive(false);
        }

        public void Show(string message)
        {
            errorText.text = message;
            popup.SetActive(true);
        }

        public void Hide()
        {
            popup.SetActive(false);
        }
    }
}