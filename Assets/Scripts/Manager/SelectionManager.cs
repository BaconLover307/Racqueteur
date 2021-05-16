using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public Image racquetDisplay1;
    public Image racquetDisplay2;
    public Image controlDisplay1;
    public Image controlDisplay2;
    public TextMeshProUGUI controlText1;
    public TextMeshProUGUI controlText2;

    [Header("Racquet Selection")]
    public Sprite[] racquets;
    public int selectedRacquet1;
    public int selectedRacquet2;

    [Header("Control Selection")]
    public Sprite[] controls;
    public string selectedControl1;
    public string selectedControl2;

    [Header("UI Components")]
    public GameObject[] buttonObjs;
    public Slider loadingBar;

    private int c1Index;
    private int c2Index;

    public void Start()
    {
        selectedRacquet1 = PlayerPrefs.GetInt("Racquet1", 0);
        selectedRacquet2 = PlayerPrefs.GetInt("Racquet2", 0);
        racquetDisplay1.sprite = racquets[selectedRacquet1];
        racquetDisplay2.sprite = racquets[selectedRacquet2];
        
        selectedControl1 = PlayerPrefs.GetString("Control1", "KeyboardLeft");
        selectedControl2 = PlayerPrefs.GetString("Control2", "KeyboardRight");
        for (int i = 0; i<controls.Length; i++)
        {
            if (selectedControl1 == controls[i].name)
            {
                c1Index = i;
            }
            if (selectedControl2 == controls[i].name)
            {
                c2Index = i;
            }
        }
        controlDisplay1.sprite = controls[c1Index];
        controlDisplay2.sprite = controls[c2Index];
        controlText1.text = selectedControl1;
        controlText2.text = selectedControl2;
    }

    public void ReadyChoosing()
    {
        PlayerPrefs.SetInt("Racquet1", selectedRacquet1);
        PlayerPrefs.SetInt("Racquet2", selectedRacquet2);
        PlayerPrefs.SetString("Control1", selectedControl1);
        PlayerPrefs.SetString("Control2", selectedControl2);

        PlayerPrefs.Save();
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        foreach(GameObject obj in buttonObjs)
        {
            obj.SetActive(false);
        }
        loadingBar.gameObject.SetActive(true);
        
        AsyncOperation load = SceneManager.LoadSceneAsync("Arena");

        while(!load.isDone)
        {
            loadingBar.value = Mathf.Clamp01(load.progress / .9f);
            yield return null;
        }
    }

    public void BackScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void nextRacquet1()
    {
        selectedRacquet1 = (selectedRacquet1 + 1) % racquets.Length;
        racquetDisplay1.sprite = racquets[selectedRacquet1];
    }

    public void prevRacquet1()
    {
        selectedRacquet1 = selectedRacquet1 - 1 < 0 ? racquets.Length - 1: selectedRacquet1 - 1;
        racquetDisplay1.sprite = racquets[selectedRacquet1];
    }

    public void nextRacquet2()
    {
        selectedRacquet2 = (selectedRacquet2 + 1) % racquets.Length;
        racquetDisplay2.sprite = racquets[selectedRacquet2];
    }

    public void prevRacquet2()
    {
        selectedRacquet2 = selectedRacquet2 - 1 < 0 ? racquets.Length - 1 : selectedRacquet2 - 1;
        racquetDisplay2.sprite = racquets[selectedRacquet2];
    }

    public void nextControl1()
    {
        do
        {
            c1Index = (c1Index + 1) % controls.Length;
            selectedControl1 = controls[c1Index].name;

        } while ((selectedControl1 != "Gamepad" && c1Index == c2Index)
                    || (selectedControl2 == "KeyboardFull" && (selectedControl1 == "KeyboardLeft" || selectedControl1 == "KeyboardRight"))
                    || (selectedControl1 == "KeyboardFull" && (selectedControl2 == "KeyboardLeft" || selectedControl2 == "KeyboardRight")));

        controlDisplay1.sprite = controls[c1Index];
        controlText1.text = selectedControl1;
    }
    public void prevControl1()
    {
        do
        {
            c1Index = c1Index - 1 < 0 ? controls.Length - 1 : c1Index - 1;
            selectedControl1 = controls[c1Index].name;

        } while ((selectedControl1 != "Gamepad" && c1Index == c2Index)
                    || (selectedControl2 == "KeyboardFull" && (selectedControl1 == "KeyboardLeft" || selectedControl1 == "KeyboardRight"))
                    || (selectedControl1 == "KeyboardFull" && (selectedControl2 == "KeyboardLeft" || selectedControl2 == "KeyboardRight")));

        controlDisplay1.sprite = controls[c1Index];
        controlText1.text = selectedControl1;
    }

    public void nextControl2()
    {
        do
        {
            c2Index = (c2Index + 1) % controls.Length;
            selectedControl2 = controls[c2Index].name;

        } while ((selectedControl2 != "Gamepad" && c1Index == c2Index)
                    || (selectedControl2 == "KeyboardFull" && (selectedControl1 == "KeyboardLeft" || selectedControl1 == "KeyboardRight"))
                    || (selectedControl1 == "KeyboardFull" && (selectedControl2 == "KeyboardLeft" || selectedControl2 == "KeyboardRight")));

        controlDisplay2.sprite = controls[c2Index];
        controlText2.text = selectedControl2;
    }
    public void prevControl2()
    {
        do
        {
            c2Index = c2Index - 1 < 0 ? controls.Length - 1 : c2Index - 1;
            selectedControl2 = controls[c2Index].name;

        } while ((selectedControl2 != "Gamepad" && c1Index == c2Index)
                    || (selectedControl2 == "KeyboardFull" && (selectedControl1 == "KeyboardLeft" || selectedControl1 == "KeyboardRight"))
                    || (selectedControl1 == "KeyboardFull" && (selectedControl2 == "KeyboardLeft" || selectedControl2 == "KeyboardRight")));

        controlDisplay2.sprite = controls[c2Index];
        controlText2.text = selectedControl2;
    }

}
