using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Manager.Options
{
    [Serializable]
    public struct PlayerControlOptions
    {
        public TMP_Text upControl;
        public TMP_Text downControl;
        public TMP_Text leftControl;
        public TMP_Text rightControl;
        public TMP_Text cwControl;
        public TMP_Text ccwControl;
        public TMP_Text flipControl;
        public TMP_Text pauseControl;
    }

    public class KeyboardSchemeButtonHandler : MonoBehaviour
    {
        private static Dictionary<string, Dictionary<string, List<string>>> _defaultControlsMap;
        public Button[] buttons;
        public List<string> names;
        public int playerIdx;
        public PlayerControlOptions controlOptions;
        public InputActionAsset playerControls;

        private void Start()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var schemeIdx = i;
                buttons[i].onClick.AddListener(delegate { ChangeScheme(names[schemeIdx]); });
            }

            ParseInputActionAsset();
            DeviceManager.Instance.OnControllerChange += OnChangeControlMapping;
            OnChangeControlMapping();
        }

        private void ChangeScheme(string schemeName)
        {
            if (!schemeName.StartsWith("Keyboard")) return;
            
            foreach (var button in buttons) button.interactable = true;
            
            var idx = names.IndexOf(schemeName);

            buttons[idx].interactable = false;
            DeviceManager.Instance.OnChangeScheme(playerIdx + "," + names[idx]);
        }

        private void ParseInputActionAsset()
        {
            if (_defaultControlsMap != null) return;

            _defaultControlsMap = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var action in playerControls.FindActionMap("Player").actions)
            {
                if (!_defaultControlsMap.ContainsKey(action.name))
                    _defaultControlsMap[action.name] = new Dictionary<string, List<string>>();

                var defaultControlAction = _defaultControlsMap[action.name];
                foreach (var inputBinding in action.bindings)
                {
                    var groups = inputBinding.groups.Split(';');
                    foreach (var group in groups)
                        switch (inputBinding.groups)
                        {
                            case "":
                                continue;
                            default:
                            {
                                if (!defaultControlAction.ContainsKey(@group))
                                    defaultControlAction[@group] = new List<string>();
                                var keyName = inputBinding.path.Split('/')[1];
                                var regex = new Regex("([a-zA-Z])([A-Z])");
                                defaultControlAction[@group]
                                    .Add(regex.Replace(keyName, "$1 $2").ToUpper());
                                break;
                            }
                        }
                }
            }
        }

        private void OnChangeControlMapping()
        {
            var (_, scheme) = DeviceMap.PlayerDevices[playerIdx];
            ChangeScheme(scheme);

            var defaultControl = new List<string>();

            if (scheme.Equals("Gamepad"))
            {
                // gamepad only have one scheme for movement
                defaultControl.AddRange(_defaultControlsMap["Movement"][scheme]);
                defaultControl.AddRange(_defaultControlsMap["Movement"][scheme]);
                defaultControl.AddRange(_defaultControlsMap["Movement"][scheme]);
            }

            defaultControl.AddRange(_defaultControlsMap["Movement"][scheme]);

            defaultControl.AddRange(_defaultControlsMap["Rotate"][scheme]);
            defaultControl.AddRange(_defaultControlsMap["Flip"][scheme]);
            defaultControl.AddRange(_defaultControlsMap["Pause"][scheme]);

            var fields = typeof(PlayerControlOptions).GetFields();
            for (var i = 0; i < fields.Length; i++)
                ((TMP_Text) fields[i].GetValue(controlOptions)).text = defaultControl[i];
        }
    }
}