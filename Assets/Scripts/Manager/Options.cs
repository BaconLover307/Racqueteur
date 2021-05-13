using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Options : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI up1, down1, left1, right1, cw1, ccw1;
    public TextMeshProUGUI up2, down2, left2, right2, cw2, ccw2;

    private GameObject currentKey;

    public void Start()
    {
        keys.Add("Up1", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Up1", "W")));
        keys.Add("Down1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down1", "S")));
        keys.Add("Left1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left1", "A")));
        keys.Add("Right1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right1", "D")));
        keys.Add("CW1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CW1", "V")));
        keys.Add("CCW1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CCW1", "N")));

        keys.Add("Up2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up2", "I")));
        keys.Add("Down2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down2", "K")));
        keys.Add("Left2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left2", "J")));
        keys.Add("Right2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right2", "L")));
        keys.Add("CW2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CW2", "LeftArrow")));
        keys.Add("CCW2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CCW2", "RightArrow")));


        up1.text = keys["Up1"].ToString();
        down1.text = keys["Down1"].ToString();
        left1.text = keys["Left1"].ToString();
        right1.text = keys["Right1"].ToString();
        cw1.text = keys["CW1"].ToString();
        ccw1.text = keys["CCW1"].ToString();

        up2.text = keys["Up2"].ToString();
        down2.text = keys["Down2"].ToString();
        left2.text = keys["Left2"].ToString();
        right2.text = keys["Right2"].ToString();
        cw2.text = keys["CW2"].ToString();
        ccw2.text = keys["CCW2"].ToString();
    }

    public void BackScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                string currentName = currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().name;

                keys[currentName] = e.keyCode;
                Debug.Log(currentName);
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();

                // Save the key binding to PlayerPrefs
                PlayerPrefs.SetString(currentName, e.keyCode.ToString());
                PlayerPrefs.Save();

                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }

    // Save all key bindings when pressed Save
    public void saveKeys()
    {
        foreach(var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }
}
