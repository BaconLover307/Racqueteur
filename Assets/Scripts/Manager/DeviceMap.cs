using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

namespace Manager
{
    public class DeviceMap : MonoBehaviour
    {
        // public static DeviceMap Instance;

        public static Dictionary<int, (InputDevice, string)> PlayerDevices;
        public int maxPlayer = 2;

        // private void Awake()
        // {
        //     if (Instance)
        //     {
        //         Debug.Log("instance");
        //         Destroy(gameObject);
        //         return;
        //     }
        //
        //     Debug.Log("Awake");
        //     Instance = this;
        //     
        //     DontDestroyOnLoad(gameObject);
        // }

        private void Start()
        {
            // TODO save device and scan on load 

            if (PlayerDevices != null) return;
            
            
            Debug.Log("called");
            // new device option
            var defaultScheme = new[] {"KeyboardLeft", "KeyboardRight"};
            PlayerDevices = new Dictionary<int, (InputDevice, string)>();

            for (var playerIdx = 0; playerIdx < maxPlayer; playerIdx++)
            {
                PlayerDevices[playerIdx] = (Keyboard.current, defaultScheme[playerIdx]);
            }
        }
    }
}