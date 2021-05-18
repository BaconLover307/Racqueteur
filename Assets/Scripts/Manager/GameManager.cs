using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Player;
using Tower;
using Ball;
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
    public Movement ball;
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

    [Header("SFX")]
    public AudioClip countdownSFX;
    public AudioClip ann60sSFX;
    public AudioClip ann30sSFX;
    public AudioClip ann10sSFX;
    public AudioClip winnerSFX;

    private List<PlayerController> players = new List<PlayerController>();
    private AudioManager _audioManager;

    private void Awake()
    {
        Instance = this;
        _audioManager = AudioManager.instance;
    }

    void Start()
    {
        StartCoroutine(TurnOnLights());
        SpawnRacquets();
        DisableControllers(true);
        _audioManager.ShutUp();
        _audioManager.PlayAnnounce(countdownSFX);
        StartCoroutine(Countdown(3, "GO!", false));

        P1Health.OnTowerDestroy += EndCondition;
        P2Health.OnTowerDestroy += EndCondition;
        timer.OnTimerEnd += EndCondition;
        timer.OnTimerNotification += TimeNotification;
        timer.OnTimerLastCountdown += CallLastCountdown;

    }

    private IEnumerator Countdown(int duration, string endString, bool isEnd)
    {
        countdownDisplay.SetActive(true);

        float initialTimestamp = timer.timeRemaining;

        TextMeshProUGUI countdownGUI = countdownDisplay.GetComponentInChildren<TextMeshProUGUI>();
        Color targetColor = countdownGUI.color;
        Color whiteColor = new Color(1, 1, 1, 1);
        float targetSize = countdownGUI.fontSize;
        float initialSize = targetSize * 1.2f;


        float elapsedTime;
        do
        {
            elapsedTime = initialTimestamp - timer.timeRemaining;

            if (elapsedTime < duration)
            {
                int countdownTime = duration - Mathf.FloorToInt(elapsedTime);
                countdownGUI.text = countdownTime.ToString();

            }
            else
            {
                countdownGUI.text = endString;
                if (isEnd)
                {
                    DisableControllers();
                    ball.FreezeBall();
                }
                else
                {
                    EnableControllers();
                }
            }

            countdownGUI.color = Color.Lerp(whiteColor, targetColor, animCurve.Evaluate(elapsedTime - Mathf.FloorToInt(elapsedTime)));
            countdownGUI.fontSize = Mathf.Lerp(initialSize, targetSize, animCurve.Evaluate(elapsedTime - Mathf.FloorToInt(elapsedTime)));

            yield return new WaitForSeconds(Time.deltaTime);
        } while (elapsedTime < duration + 1);

        countdownDisplay.SetActive(false);
        countdownGUI.color = targetColor;
        countdownGUI.fontSize = targetSize;
        
        timer.displayTimer = true;
    }

    private void CallLastCountdown()
    {
        _audioManager.PlayAnnounce(ann10sSFX);
        StartCoroutine(Countdown(10, "Time's Up", true));
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
        var timeRemaining = Math.Floor(timer.timeRemaining);

        if (timeRemaining == 30)
        {
            _audioManager.PlayAnnounce(ann30sSFX);
            notificationDisplay.text = "30 Seconds Remaining";
        }
        if (timeRemaining == 60)
        {
            _audioManager.PlayAnnounce(ann60sSFX);
            notificationDisplay.text = "1 Minute Remaining";
        }

        StartCoroutine(ShowTimeNotification());
    }

    IEnumerator ShowTimeNotification()
    {
        notificationDisplay.gameObject.SetActive(true);
        notificationDisplay.gameObject.GetComponent<Animator>().Play("Announcement");
        yield return new WaitForSeconds(4.0f);
        notificationDisplay.gameObject.SetActive(false);
    }

    private IEnumerator ShowEndGameScreen()
    {
        yield return new WaitForSeconds(3.0f);
        _audioManager.PlayAnnounce(winnerSFX);
        notificationDisplay.gameObject.SetActive(false);
        countdownDisplay.SetActive(false);
        EndGameScreen.SetActive(true);
        EndGameScreen.GetComponent<Animator>().Play("EndMenu");
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
            pInput.GetComponentInChildren<PlayerController>().FreezeRacquet();
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
        // Set timescale to make the title animation run
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Arena");
    }

    #endregion
}
