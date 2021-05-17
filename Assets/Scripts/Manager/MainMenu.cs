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
        float musicVol = PlayerPrefs.GetFloat("musicVol", 1f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVol", 1f);
        float annVol = PlayerPrefs.GetFloat("annVol", 1f);

        mainMix.SetFloat("musicVol", Mathf.Log10(musicVol) * 20);
        mainMix.SetFloat("sfxVol", Mathf.Log10(sfxVol) * 20);
        mainMix.SetFloat("annVol", Mathf.Log10(annVol) * 20);

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
        //creditsPanel.SetActive(false);

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        //creditsPanel.SetActive(true);
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
        //panel.GetComponent<Canvas>().enabled = false;
        new WaitForSeconds(0.5f);
        panel.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        //panel.GetComponent<Canvas>().enabled = true;
        panel.SetActive(true);
    }
}
