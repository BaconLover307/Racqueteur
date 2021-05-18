using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject tutorialsPanel;
    public GameObject creditsPanel;

    [Header("Audio")]
    public AudioMixer mainMix;


    void Start()
    {
        // Set volume sound
        float masterVol = PlayerPrefs.GetFloat("masterVol", 0.5f);
        float musicVol = PlayerPrefs.GetFloat("musicVol", 0.35f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVol", 0.75f);
        float annVol = PlayerPrefs.GetFloat("annVol", 0.6f);

        mainMix.SetFloat("masterVol", Mathf.Log10(masterVol) * 20);
        mainMix.SetFloat("musicVol", Mathf.Log10(musicVol) * 20);
        mainMix.SetFloat("sfxVol", Mathf.Log10(sfxVol) * 20);
        mainMix.SetFloat("annVol", Mathf.Log10(annVol) * 20);

        AudioManager.instance.Play("MenuMusic");

        DisablePanels();
        ShowPanel(mainMenuPanel);
    }

    public void DisablePanels()
    {
        HidePanel(mainMenuPanel);
        HidePanel(tutorialsPanel);
        HidePanel(creditsPanel);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SelectionScene");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void OpenTutorials()
    {
        DisablePanels();
        ShowPanel(tutorialsPanel);
    }

    public void OpenCredits()
    {
        DisablePanels();

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        ShowPanel(creditsPanel);
    }

    public void BackToMainMenu()
    {
        DisablePanels();
        ShowPanel(mainMenuPanel);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
}
