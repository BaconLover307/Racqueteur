using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Manager;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public Image[] racquetDisplays;
    public Image[] controlDisplays;
    public TextMeshProUGUI[] controlTexts;

    [Header("Racquet Selection")]
    public Sprite[] racquets;
    public int[] selectedRacquets;

    [Header("Control Selection")]
    public List<Sprite> controls;

    [Header("UI Components")]
    public GameObject[] buttonObjs;
    public Slider loadingBar;

    public void Start()
    {
        for (int i = 0; i < selectedRacquets.Length; i++)
        {
            selectedRacquets[i] = PlayerPrefs.GetInt("Racquet" + i, 0);
            racquetDisplays[i].sprite = racquets[selectedRacquets[i]];

            var scheme = DeviceMap.PlayerDevices[i].Item2;
            controlTexts[i].text = scheme;
            controlDisplays[i].sprite = controls.Find(control => control.name.Equals(scheme));
        }
    }

    public void ReadyChoosing()
    {
        for (int i = 0; i < selectedRacquets.Length; i++)
        {
            PlayerPrefs.SetInt("Racquet"+i, selectedRacquets[i]);
        }

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

    public void SelectRacquet(string conf)
    {
        var options = conf.Split(',');

        var idx = int.Parse(options[1]);
        if (options[0].Equals("Prev"))
        {
            selectedRacquets[idx] = (selectedRacquets[idx] + racquets.Length - 1) % racquets.Length;

        }
        else if (options[0].Equals("Next"))
        {
            selectedRacquets[idx] = (selectedRacquets[idx] + 1) % racquets.Length;
        }
        racquetDisplays[idx].sprite = racquets[selectedRacquets[idx]];
    }


}
