using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Settings")]
    public PlayerController[] racquets;
    public GameObject[] spawnPoints;
    public Material P1Mat;
    public Material P2Mat;


    [Header("Light Settings")]
    public ArenaLight arenaLight;
    public TowerLight[] towerLights;
    public BorderLight[] borderLights;

    private KeyboardSplitter keyboardSplitter;
    private PlayerController player1;
    private PlayerController player2;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        //keyboardSplitter = GetComponent<KeyboardSplitter>();

        // Set key binding from PlayerPrefs
        // TODO: Change parameter kedua dari PlayerPrefs.GetString ke variable remapping yang ada di KeyboardSplitter
        //       & assign variabel remapping dengan keyCode-keyCode di bawah
        //Key remappedUp1 = keyboardSplitter.players[0].routes[0].remapped;
        //Key remappedDown1 = keyboardSplitter.players[0].routes[1].remapped;
        //Key remappedLeft1 = keyboardSplitter.players[0].routes[2].remapped;
        //Key remappedRight1 = keyboardSplitter.players[0].routes[3].remapped;
        //Key remappedCW1 = keyboardSplitter.players[0].routes[4].remapped;
        //Key remappedCCW1 = keyboardSplitter.players[0].routes[5].remapped;
        //Key remappedFlip1 = keyboardSplitter.players[0].routes[6].remapped;

        //Key remappedUp2 = keyboardSplitter.players[1].routes[0].remapped;
        //Key remappedDown2 = keyboardSplitter.players[1].routes[1].remapped;
        //Key remappedLeft2 = keyboardSplitter.players[1].routes[2].remapped;
        //Key remappedRight2 = keyboardSplitter.players[1].routes[3].remapped;
        //Key remappedCW2 = keyboardSplitter.players[1].routes[4].remapped;
        //Key remappedCCW2 = keyboardSplitter.players[1].routes[5].remapped;
        //Key remappedFlip2 = keyboardSplitter.players[1].routes[6].remapped;

        //Key keyUp1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Up1", remappedUp1.ToString()));
        //Key keyDown1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Down1", remappedDown1.ToString()));
        //Key keyLeft1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Left1", remappedLeft1.ToString()));
        //Key keyRight1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Right1", remappedRight1.ToString()));
        //Key keyCW1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("CW1", remappedCW1.ToString()));
        //Key keyCCW1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("CCW1", remappedCCW1.ToString()));
        //Key keyFlip1 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Flip1", remappedFlip1.ToString()));

        //Key keyUp2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Up2", remappedUp2.ToString()));
        //Key keyDown2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Down2", remappedDown2.ToString()));
        //Key keyLeft2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Left2", remappedLeft2.ToString()));
        //Key keyRight2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Right2", remappedRight2.ToString()));
        //Key keyCW2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("CW2", remappedCW2.ToString()));
        //Key keyCCW2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("CCW2", remappedCCW2.ToString()));
        //Key keyFlip2 = (Key)System.Enum.Parse(typeof(Key), PlayerPrefs.GetString("Flip2", remappedFlip2.ToString()));

        //keyboardSplitter.players[0].routes[0].remapped = keyUp1;
        //keyboardSplitter.players[0].routes[1].remapped = keyDown1;
        //keyboardSplitter.players[0].routes[2].remapped = keyLeft1;
        //keyboardSplitter.players[0].routes[3].remapped = keyRight1;
        //keyboardSplitter.players[0].routes[4].remapped = keyCW1;
        //keyboardSplitter.players[0].routes[5].remapped = keyCCW1;
        //keyboardSplitter.players[0].routes[6].remapped = keyFlip1;

        //keyboardSplitter.players[1].routes[0].remapped = keyUp2;
        //keyboardSplitter.players[1].routes[1].remapped = keyDown2;
        //keyboardSplitter.players[1].routes[2].remapped = keyLeft2;
        //keyboardSplitter.players[1].routes[3].remapped = keyRight2;
        //keyboardSplitter.players[1].routes[4].remapped = keyCW2;
        //keyboardSplitter.players[1].routes[5].remapped = keyCCW2;
        //keyboardSplitter.players[1].routes[6].remapped = keyFlip2;

        //keyboardSplitter.UpdateRoutes();

        // Set prefab for player 1 & player 2
        int racquet1 = PlayerPrefs.GetInt("Racquet1");
        int racquet2 = PlayerPrefs.GetInt("Racquet2");

        Debug.Log(racquet1.ToString());
        Debug.Log(racquet2.ToString());


        StartCoroutine(TurnOnLights());
        SpawnRacquets();
    }

    IEnumerator TurnOnLights()
    {
        arenaLight.Initialize();
        foreach (BorderLight light in borderLights)
        {
            light.Initialize();
        }
        yield return new WaitForSeconds(arenaLight.sweepDuration);
        foreach (TowerLight light in towerLights)
        {
            light.Initialize();
        }
    }

    private void SpawnRacquets()
    {
        player1 = Instantiate(racquets[PlayerPrefs.GetInt("Racquet1")], spawnPoints[0].transform);
        player1.GetComponent<RacketLight>().UpdateMaterial(P1Mat);
        PlayerInput p1Controller = player1.GetComponent<PlayerInput>();
        p1Controller.SwitchCurrentControlScheme(PlayerPrefs.GetString("Control1", "KeyboardLeft"));


        player2 = Instantiate(racquets[PlayerPrefs.GetInt("Racquet2")], spawnPoints[1].transform);
        player2.GetComponent<RacketLight>().UpdateMaterial(P2Mat);
        PlayerInput p2Controller = player2.GetComponent<PlayerInput>();
        p2Controller.SwitchCurrentControlScheme(PlayerPrefs.GetString("Control2", "KeyboardRight"));
    }

    #region public callback

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public void OnRestart()
    {
        SceneManager.LoadScene("Arena");
    }

    #endregion
}
