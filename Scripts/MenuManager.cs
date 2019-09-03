using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Animation Uianim;

    void Start()
    {
        EventManager.StartGameHandler += StartGame;
        EventManager.PostStopGameHandler += StopGame;
        EventManager.PauseGameHandler += (p) => { if (!p) StartGame(); else StopGame(); };
    }

    void StartGame()
    {
        Uianim["StartGame"].speed = FPSSpeedData.AnimationSpeed;

        Uianim.Play("StartGame");
    }

    void StopGame()
    {
        Uianim["StopGame"].speed = FPSSpeedData.AnimationSpeed;

        Uianim.Play("StopGame");
    }
}