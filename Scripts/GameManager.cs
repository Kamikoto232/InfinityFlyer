using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    float timer = 1.55f;
    bool StartTimer;
    public bool InGame, Pause;
    public UnityEvent OnStartGame, OnStopGame, OnPauseGame, OnUnpauseGame;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enabled = false;
    }

    public void Startgame()
    {
        timer = 1f;
        InGame = true;
        StartTimer = true;
        OnStartGame?.Invoke();
        EventManager.OnStartGame();
        enabled = true;
    }

    public void Stopgame()
    {
        timer = 1.55f;
        OnStopGame?.Invoke();
        EventManager.OnStopGame();

        enabled = true;
    }

    public void PauseGame()
    {
        Pause = !Pause;
        Time.timeScale = Pause ? 0.001f : 1;
        EventManager.OnPauseGame(Pause);
        if(Pause) OnPauseGame?.Invoke();
        else OnUnpauseGame?.Invoke();
    }

    private void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            if (StartTimer) EventManager.OnPostStartGame();
            else
            {
                EventManager.OnPostStopGame();
                InGame = false;
            }

            StartTimer = false;
            enabled = false;
        }
    }
}