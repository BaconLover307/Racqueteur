using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Manager.Options
{
    public class KeyboardSchemeButtonHandler : MonoBehaviour
    {
        public Button[] buttons;
        public List<string> names;
        public int playerIdx;
        public InputActionAsset playerControls;

        private Dictionary<string, List<string>> _defaultControls;

        private void Start()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var idx = i;
                buttons[i].onClick.AddListener(delegate { ChangeScheme(idx); });
            }

            ParseInputActionAsset();

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

        private void ParseInputActionAsset()
        {
            _defaultControls = new Dictionary<string, List<string>>();
            foreach (var inputBinding in playerControls.FindActionMap("Player").FindAction("Movement").bindings)
            {
                if (inputBinding.groups.Equals("")) continue;

                if (inputBinding.groups.Equals("Gamepad"))
                {
                    _defaultControls["Gamepad"] = new List<string> {inputBinding.path.Split('/')[1]};
                }
                else
                {
                    if (!_defaultControls.ContainsKey(inputBinding.groups))
                        _defaultControls[inputBinding.groups] = new List<string>();
                    _defaultControls[inputBinding.groups].Add(inputBinding.path.Split('/')[1]);
                }
            }
        }
    }
}