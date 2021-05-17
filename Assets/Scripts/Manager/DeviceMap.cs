using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class DeviceMap : MonoBehaviour
    {
        public static Dictionary<int, (InputDevice, string)> PlayerDevices;
        public string[] defaultScheme;

        #region unity callback

        private void Start()
        {
            // TODO save device and scan on load 
            if (PlayerDevices != null) return;

            PlayerDevices = new Dictionary<int, (InputDevice, string)>();
            CheckDevice();
        }

        #endregion

        #region public callback

        public static void SavePlayerDevice()
        {
            for (int playerIdx = 0; playerIdx < PlayerDevices.Count; playerIdx++)
            {
                PlayerPrefs.SetInt("deviceId" + playerIdx, PlayerDevices[playerIdx].Item1.deviceId);
                PlayerPrefs.SetString("control" + playerIdx, PlayerDevices[playerIdx].Item2);
            }
        }

        #endregion

        #region private function

        private void CheckDevice()
        {
            var deviceList = InputSystem.devices.ToList();

            for (var playerIdx = 0; playerIdx < defaultScheme.Length; playerIdx++)
            {
                var playerDevice = deviceList.Find(device =>
                    device.deviceId.Equals(PlayerPrefs.GetInt("deviceId" + playerIdx, -1)));
                
                if (playerDevice != null)
                    PlayerDevices[playerIdx] = (playerDevice,
                        PlayerPrefs.GetString("control" + playerIdx, defaultScheme[playerIdx]));
                else
                    PlayerDevices[playerIdx] = (Keyboard.current, defaultScheme[playerIdx]);
            }
        }

        #endregion
    }
}