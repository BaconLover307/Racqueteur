using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    void Start()
    {
        DisablePanels();
        ShowPanel(mainMenuPanel);
    }

    public void DisablePanels()
    {
        HidePanel(mainMenuPanel);
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

    public void OpenCredits()
    {
        DisablePanels();
        creditsPanel.SetActive(false);
        creditsPanel.SetActive(true);
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
        panel.GetComponent<Canvas>().enabled = false;
    }

    public void ShowPanel(GameObject panel)
    {
        panel.GetComponent<Canvas>().enabled = true;
    }
}
