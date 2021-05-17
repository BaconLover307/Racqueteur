using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

namespace Manager
{
    public class DeviceManager : MonoBehaviour
    {
        public static DeviceManager Instance;

        public GameObject playerPrefabs;
        public TMP_Text[] devicesText;
        public GameObject[] keyboardScheme;
        public Button[] bindButton;
        public int maxPlayer = 2;

        public bool isValidScheme;
        private int _currPlayerIndex = -1;

        private InputAction _myAction;

        private GameObject[] _playerInstances;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _playerInstances = new[]
            {
                Instantiate(playerPrefabs),
                Instantiate(playerPrefabs)
            };
            var playerInputs = _playerInstances.Select(player => player.GetComponent<PlayerInput>()).ToList();

            // device option already exist
            for (var playerIdx = 0; playerIdx < maxPlayer; playerIdx++)
            {
                var (device, scheme) = DeviceMap.PlayerDevices[playerIdx];
                var playerInput = playerInputs[playerIdx];

                playerInput.SwitchCurrentControlScheme(scheme);
                InputUser.PerformPairingWithDevice(device, playerInput.user,
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser);
                devicesText[playerIdx].text = device.displayName;

                if (!scheme.StartsWith("Keyboard")) keyboardScheme[playerIdx].SetActive(false);
            }

            // This subscribes us to events that will fire if any button is pressed.  We'll most certainly want to throw this away 
            // when not in a selection screen (performance intensive)!
            _myAction = new InputAction(binding: "/*/<button>");
            _myAction.performed += action =>
            {
                // playerInputManager.JoinPlayerFromActionIfNotAlreadyJoined(action);
                AddPlayer(action.control.device);
            };
            _myAction.Enable();
            CheckValidScheme();
        }

        private void OnDestroy()
        {
            _myAction.Dispose();
        }

        public void OnBind(int playerIndex)
        {
            bindButton[playerIndex].interactable = false;
            _currPlayerIndex = playerIndex;
            devicesText[playerIndex].text = "PRESS ANY KEY";
            _playerInstances[playerIndex].GetComponent<PlayerInput>().user.UnpairDevices();
        }

        public void OnChangeScheme(string args)
        {
            var splitArgs = args.Split(',');
            var playerIdx = int.Parse(splitArgs[0]);
            var scheme = splitArgs[1];
            var (inputDevice, _) = DeviceMap.PlayerDevices[playerIdx];
            var playerInstance = _playerInstances[playerIdx];

            playerInstance.GetComponent<PlayerInput>().SwitchCurrentControlScheme(scheme);
            DeviceMap.PlayerDevices[playerIdx] = (inputDevice, scheme);
            CheckValidScheme();
        }

        private void AddPlayer(InputDevice device)
        {
            if (_currPlayerIndex.Equals(-1)) return;

            // Don't execute if not a gamepad or keyboard
            if (!device.displayName.Contains("Controller") && !device.displayName.Contains("Gamepad") &&
                !device.displayName.Contains("Keyboard"))
                return;

            if (PlayerInput.all.SelectMany(player => player.devices)
                .Any(playerDevice => device == playerDevice && !device.displayName.Contains("Keyboard"))) return;

            var playerInput = _playerInstances[_currPlayerIndex].GetComponent<PlayerInput>();
            if (device.displayName.Contains("Keyboard"))
            {
                keyboardScheme[_currPlayerIndex].SetActive(true);
                var scheme = _currPlayerIndex == 0 ? "KeyboardLeft" : "KeyboardRight";
                playerInput.SwitchCurrentControlScheme(scheme);
                InputUser.PerformPairingWithDevice(Keyboard.current, playerInput.user,
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser);
                DeviceMap.PlayerDevices[_currPlayerIndex] = (device, scheme);
            }
            else
            {
                keyboardScheme[_currPlayerIndex].SetActive(false);
                playerInput.SwitchCurrentControlScheme("Gamepad");
                InputUser.PerformPairingWithDevice(device, playerInput.user,
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser);
                DeviceMap.PlayerDevices[_currPlayerIndex] = (device, "Gamepad");
            }

            devicesText[_currPlayerIndex].text = device.displayName;
            bindButton[_currPlayerIndex].interactable = true;
            CheckValidScheme();

            _currPlayerIndex = -1;
        }

        private void CheckValidScheme()
        {
            var scheme1 = DeviceMap.PlayerDevices[0].Item2;
            var scheme2 = DeviceMap.PlayerDevices[1].Item2;

            isValidScheme = scheme1 != scheme2;

            if (scheme1.StartsWith("Keyboard") && scheme2.StartsWith("Keyboard") &&
                (scheme1.EndsWith("Full") || scheme2.EndsWith("Full")))
                isValidScheme = false;
            else if (scheme1.Equals("Gamepad") && scheme2.Equals("Gamepad")) isValidScheme = true;
        }
    }
}