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
    public GameObject EndGameScreen;
    public Timer timer;
    public string monoSpacingSize = "30";
    public GameObject countdownDisplay;
    public TextMeshProUGUI winnerDisplay;
    public TextMeshProUGUI notificationDisplay;

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
        DisableControllers(true);
        StartCoroutine(Countdown());

        P1Health.OnTowerDestroy += EndCondition;
        P2Health.OnTowerDestroy += EndCondition;
        timer.OnTimerEnd += EndCondition;
        timer.OnTimerNotification += TimeNotification;
        timer.OnTimerLastCountdown += CallLastCountdown;
    }

    private IEnumerator Countdown()
    {
        int countdownTime = 3;
        TextMeshProUGUI countdownGUI = countdownDisplay.GetComponentInChildren<TextMeshProUGUI>();
        while (countdownTime > 0)
        {
            countdownGUI.text = $"<mspace=mspace={monoSpacingSize}>{countdownTime.ToString()}</mspace>";
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownGUI.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownDisplay.SetActive(false);
        EnableControllers();
        timer.StartTimer();
    }

    private void CallLastCountdown()
    {
        StartCoroutine(LastCountdown());
    }
    private IEnumerator LastCountdown()
    {
        countdownDisplay.gameObject.SetActive(true);
        int countdownTime = 10;
        while (countdownTime > 0)
        {
            TextMeshProUGUI countdownGUI = countdownDisplay.GetComponentInChildren<TextMeshProUGUI>();
            countdownGUI.text = $"<mspace=mspace={monoSpacingSize}>{countdownTime.ToString()}</mspace>";
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        countdownDisplay.gameObject.SetActive(false);

        DisableControllers(true);
        notificationDisplay.text = "Time's Up";
        notificationDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
    }

    private void EndCondition()
    {
        if (P1Health.health > P2Health.health)
        {
            winnerDisplay.text = "PLAYER 1 WINS";
            winnerDisplay.color = new Color32(214, 73, 69, 255);
        }
        else if (P2Health.health >= P1Health.health)
        {
            winnerDisplay.text = "PLAYER 2 WINS";
            winnerDisplay.color = new Color32(69, 123, 214, 255);
        }
        timer.StopTimer();
        StartCoroutine(ShowEndGameScreen());
    }

    private void TimeNotification()
    {
        notificationDisplay.text = Math.Floor(timer.timeRemaining).ToString() + " Seconds Remaining";
        StartCoroutine(ShowTimeNotification());
    }

    IEnumerator ShowTimeNotification()
    {
        notificationDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        notificationDisplay.gameObject.SetActive(false);
    }

    private IEnumerator ShowEndGameScreen()
    {
        yield return new WaitForSeconds(3.0f);
        notificationDisplay.gameObject.SetActive(false);
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

    private void DisableControllers(bool movementOnly = false)
    {
        PlayerInput p1 = player1.gameObject.GetComponent<PlayerInput>();
        PlayerInput p2 = player2.gameObject.GetComponent<PlayerInput>();
        if (movementOnly)
        {
            p1.actions.FindAction("Movement").Disable();
            p2.actions.FindAction("Movement").Disable();
        }
        else
        {
            p1.DeactivateInput();
            p2.DeactivateInput();
        }
    }

    private void EnableControllers()
    {
        PlayerInput p1 = player1.gameObject.GetComponent<PlayerInput>();
        PlayerInput p2 = player2.gameObject.GetComponent<PlayerInput>();
        p1.actions.FindAction("Movement").Enable();
        p2.actions.FindAction("Movement").Enable();
        p1.ActivateInput();
        p2.ActivateInput();
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
