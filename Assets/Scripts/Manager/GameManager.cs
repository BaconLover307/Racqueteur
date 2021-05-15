using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Player;
using Tower;
using Manager;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public PlayerController[] racquets;
    public GameObject[] spawnPoints;
    public Material P1Mat;
    public Material P2Mat;
    public TowerHealth P1Health;
    public TowerHealth P2Health;
    public Boolean TimeUp = false;
    public GameObject EndGameScreen;
    public Timer timer;
    public TextMeshProUGUI countdownDisplay;
    public TextMeshProUGUI winnerDisplay;

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
        StartCoroutine(TurnOnLights());
        SpawnRacquets();
        StartCoroutine(Countdown());

        P1Health.OnTowerDestroy += EndCondition;
        P2Health.OnTowerDestroy += EndCondition;
        timer.OnTimerEnd += EndCondition;
    }

    private IEnumerator Countdown()
    {
        int countdownTime = 3;
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
        timer.StartTimer();
    }

    private void EndCondition()
    {
        if (P1Health.health > P2Health.health)
        {
            winnerDisplay.text = "PLAYER 1 WIN";
        }
        else if (P2Health.health >= P1Health.health)
        {
            winnerDisplay.text = "PLAYER 2 WIN";
        }
        timer.StopTimer();
        EndGameScreen.SetActive(true);
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
        player1 = Instantiate(racquets[PlayerPrefs.GetInt("Racquet1", 0)], spawnPoints[0].transform);
        player1.GetComponent<RacketLight>().UpdateMaterial(P1Mat);
        PlayerInput p1Controller = player1.GetComponent<PlayerInput>();
        p1Controller.SwitchCurrentControlScheme(PlayerPrefs.GetString("Control1", "KeyboardLeft"));

        player2 = Instantiate(racquets[PlayerPrefs.GetInt("Racquet2", 0)], spawnPoints[1].transform);
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
