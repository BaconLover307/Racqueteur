using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Options
{
    public class Options : MonoBehaviour
    {
        public GameObject controlsView;
        public GameObject soundsView;


        public void Start()
        {
            // Disable all views
            DisableViews();
            OpenControls();
        }

        public void OnBackButton()
        {
            Debug.Log("p1: " + DeviceMap.PlayerDevices[0].Item2);
            Debug.Log("p2: " + DeviceMap.PlayerDevices[1].Item2);

            if (!DeviceManager.Instance.isValidScheme)
            {
                // TODO handle error invalid scheme
                Debug.Log("Invalid scheme");
                return;
            }

            DeviceMap.SavePlayerDevice();

            SceneManager.LoadScene("MainMenuScene");
        }

        private void DisableViews()
        {
            controlsView.SetActive(false);
            soundsView.SetActive(false);
        }

        public void OpenControls()
        {
            DisableViews();
            controlsView.SetActive(true);
        }

        public void OpenSounds()
        {
            DisableViews();
            soundsView.SetActive(true);
        }
    }
}