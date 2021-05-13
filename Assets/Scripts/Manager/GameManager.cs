using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Light Settings")]
    public ArenaLight arenaLight;
    public TowerLight[] towerLights;
    public BorderLight[] borderLights;

    public GameObject globalVolume;

    private KeyboardSplitter keyboardSplitter;
    private bool effectsOn;

    void Start()
    {
        keyboardSplitter = GetComponent<KeyboardSplitter>();

        // Set key binding from PlayerPrefs
        // TODO: Change parameter kedua dari PlayerPrefs.GetString ke variable remapping yang ada di KeyboardSplitter
        //       & assign variabel remapping dengan keyCode-keyCode di bawah
        KeyCode keyUp1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up1", "W"));
        KeyCode keyDown1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down1", "S"));
        KeyCode keyLeft1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left1", "A"));
        KeyCode keyRight1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right1", "D"));
        KeyCode keyCW1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CW1", "V"));
        KeyCode keyCCW1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CCW1", "N"));

        KeyCode keyUp2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up2", "I"));
        KeyCode keyDown2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down2", "K"));
        KeyCode keyLeft2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left2", "J"));
        KeyCode keyRight2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right2", "L"));
        KeyCode keyCW2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CW2", "LeftArrow"));
        KeyCode keyCCW2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CCW2", "RightArrow"));

        Debug.Log(keyUp1);
        Debug.Log(keyDown1);
        Debug.Log(keyLeft1);
        Debug.Log(keyRight1);
        Debug.Log(keyCW1);
        Debug.Log(keyCCW1);

        Debug.Log(keyUp2);
        Debug.Log(keyDown2);
        Debug.Log(keyLeft2);
        Debug.Log(keyRight2);
        Debug.Log(keyCW2);
        Debug.Log(keyCCW2);

        // Set shininess volume
        globalVolume.GetComponent<Volume>().weight = PlayerPrefs.GetFloat("ShininessVolume", 0);

        // Set effects
        //globalVolume.GetComponent<Bloom>().active = PlayerPrefs.GetInt("Effects", 0) == 1;
        effectsOn = PlayerPrefs.GetInt("Effects", 1) == 1;
        Debug.Log(effectsOn);

        StartCoroutine(TurnOnLights());
    }

    IEnumerator TurnOnLights()
    {
        arenaLight.Initialize();
        foreach (BorderLight light in borderLights)
        {
            light.Initialize();
        }
        yield return new WaitForSeconds(arenaLight.sweepDuration + arenaLight.lightsUpDuration);
        foreach (TowerLight light in towerLights)
        {
            light.Initialize();
        }
    }
}
