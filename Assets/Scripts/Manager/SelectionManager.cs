using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public GameObject racquet;

    public void ReadyChoosing()
    {
        int racquet1 = racquet.GetComponent<RacquetSelection>().selectedRacquet1;
        int racquet2 = racquet.GetComponent<RacquetSelection>().selectedRacquet2;

        PlayerPrefs.SetInt("Racquet1", racquet1);
        PlayerPrefs.SetInt("Racquet2", racquet2);

        PlayerPrefs.Save();
        SceneManager.LoadScene("Arena");
    }

    public void BackScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
