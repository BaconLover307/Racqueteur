using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class KeyboardSchemeButtonHandler : MonoBehaviour
    {
        public Button[] buttons;
        public List<string> names; 
        public int playerIdx;

        private void Start()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var idx = i;
                buttons[i].onClick.AddListener(delegate { ChangeScheme(idx); });
            }

            if (DeviceMap.PlayerDevices != null)
            {
                // not default options
                var (_, scheme) = DeviceMap.PlayerDevices[playerIdx];
                if (!scheme.StartsWith("Keyboard")) return;
                
                foreach (var button in buttons) button.interactable = true;
                var idx = names.IndexOf(scheme);
                buttons[idx].interactable = false;
            }
        }

        private void ChangeScheme(int idx)
        {
            foreach (var button in buttons) button.interactable = true;

            buttons[idx].interactable = false;
            DeviceManager.Instance.OnChangeScheme(playerIdx + "," + names[idx]);
        }
    }
}