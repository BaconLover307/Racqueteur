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
            if (!DeviceManager.Instance.isValidScheme)
            {
                ErrorPopup.Instance.Show("Error!!\n\n<size=50>wrong controller scheme pair</size>");
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