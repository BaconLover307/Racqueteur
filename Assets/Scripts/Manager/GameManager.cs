using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
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
    public Material[] PlayerMats;
    public TowerHealth P1Health;
    public TowerHealth P2Health;
    public Timer timer;
    public GameObject EndGameScreen;
    public string monoSpacingSize = "30";
    public GameObject countdownDisplay;
    public TextMeshProUGUI winnerDisplay;
    public TextMeshProUGUI notificationDisplay;
    public AnimationCurve animCurve;

    [Header("Light Settings")]
    public ArenaLight arenaLight;
    public TowerLight[] towerLights;
    public BorderLight[] borderLights;

    private List<PlayerController> players = new List<PlayerController>();

    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        StartCoroutine(TurnOnLights());
        SpawnRacquets();
        DisableControllers(true);
        StartCoroutine(Countdown(3, "GO!", true));

        P1Health.OnTowerDestroy += EndCondition;
        P2Health.OnTowerDestroy += EndCondition;
        timer.OnTimerEnd += EndCondition;
        timer.OnTimerNotification += TimeNotification;
        timer.OnTimerLastCountdown += CallLastCountdown;
    }

    private IEnumerator Countdown(int duration, string endString, bool isOpening)
    {
        countdownDisplay.SetActive(true);
        TextMeshProUGUI countdownGUI = countdownDisplay.GetComponentInChildren<TextMeshProUGUI>();
        Animator anim = countdownDisplay.GetComponent<Animator>();
        Color targetColor = countdownGUI.color;
        Color whiteColor = new Color(1, 1, 1, 1);
        float initialSize = countdownGUI.fontSize;
        float targetSize = 0.8f * initialSize;

        float currentTime = 0;
        while (currentTime < duration + 1)
        {
            if (currentTime < duration)
            {
                int countdownTime = duration - Mathf.FloorToInt(currentTime);
                countdownGUI.text = $"<mspace=mspace={monoSpacingSize}>{countdownTime}</mspace>";
            }
            else if (isOpening)
            {
                countdownGUI.text = endString;
            }
            else
            {
                break;
            }

            // countdownGUI.color = Color.Lerp(whiteColor, targetColor, animCurve.Evaluate(currentTime - Mathf.FloorToInt(currentTime)));
            // countdownGUI.fontSize = Mathf.Lerp(initialSize, targetSize, animCurve.Evaluate(currentTime - Mathf.FloorToInt(currentTime)));
            anim.Play("Countdown", -1, 0f);

            currentTime += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }

        countdownDisplay.SetActive(false);

        if (isOpening)
        {
            EnableControllers();
            timer.StartTimer();
        }
        else
        {
            notificationDisplay.text = endString;
            StartCoroutine(ShowTimeNotification());
        }
    }

    private void CallLastCountdown()
    {
        StartCoroutine(Countdown(10, "Time's Up", false));
    }

    private void EndCondition()
    {
        if (P1Health.health > P2Health.health)
        {
            winnerDisplay.text = "PLAYER 1 WINS";
            winnerDisplay.color = new Color32(214, 73, 69, 255);
        }
        else if (P2Health.health > P1Health.health)
        {
            winnerDisplay.text = "PLAYER 2 WINS";
            winnerDisplay.color = new Color32(69, 123, 214, 255);
        }
        else
        {
            winnerDisplay.text = "TIE!";
            winnerDisplay.color = new Color32(223, 158, 255, 255);
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
        yield return new WaitForSeconds(2.0f);
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
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            PlayerController player = Instantiate(racquets[PlayerPrefs.GetInt("Racquet" + i, 0)], spawnPoints[i].transform);
            player.GetComponent<RacketLight>().UpdateMaterial(PlayerMats[i]);
            PlayerInput controller = player.GetComponent<PlayerInput>();
            controller.SwitchCurrentControlScheme(DeviceMap.PlayerDevices[i].Item2);
            InputUser.PerformPairingWithDevice(DeviceMap.PlayerDevices[i].Item1, controller.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
            players.Add(player);
        }
    }

    private void DisableControllers(bool movementOnly = false)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            PlayerInput pInput = players[i].GetComponent<PlayerInput>();
            if (movementOnly)
            {
                pInput.actions.FindAction("Movement").Disable();
            }
            else
            {
                pInput.DeactivateInput();
            }
        }
    }

    private void EnableControllers()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            PlayerInput pInput = players[i].GetComponent<PlayerInput>();
            pInput.actions.FindAction("Movement").Enable();
            pInput.ActivateInput();
        }
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
