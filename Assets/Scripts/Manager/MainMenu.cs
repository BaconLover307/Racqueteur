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
        mainMenuPanel.SetActive(true);
    }

    public void DisablePanels()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
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
        creditsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        DisablePanels();
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
