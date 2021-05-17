using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Manager
{
    public class PlayerSelectionController : MonoBehaviour
    {
        public GameObject prefab;

        private void Start()
        {
            for (var playerIdx = 0; playerIdx < DeviceMap.PlayerDevices.Count; playerIdx++)
            {
                var player = Instantiate(prefab);
                var playerInput = player.GetComponent<PlayerInput>();
                playerInput.SwitchCurrentActionMap("UI");
                playerInput.SwitchCurrentControlScheme(DeviceMap.PlayerDevices[playerIdx].Item2);
                InputUser.PerformPairingWithDevice(DeviceMap.PlayerDevices[playerIdx].Item1, playerInput.user,
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser);

                var idx = playerIdx;
                playerInput.onActionTriggered += context =>
                {
                    if (!context.action.name.Equals("Move") || context.phase != InputActionPhase.Performed) return;

                    var val = context.ReadValue<float>();
                    if (val < 0)
                        SelectionManager.Instance.SelectRacquet("Prev," + idx);
                    else if (val > 0) SelectionManager.Instance.SelectRacquet("Next," + idx);
                };
            }
        }
    }
}