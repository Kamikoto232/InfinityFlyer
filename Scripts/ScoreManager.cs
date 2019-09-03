using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager sm;
    public TMP_Text ScoreT, InfRecordT;
    public int Score;
    public int Record;
    public static bool NewRecord { get; private set; }
    bool StageMode;
    StageInfo CurrentStage;

    void Awake()
    {
        sm = this;
    }

    private void Start()
    {
        CheckInfinityRecord();
        EventManager.StopGameHandler += StopGame;
        EventManager.PostStartGameHandler += sm.StartGame;
    }

    void StartGame()
    {
        Score = -2;
        InvokeRepeating("AddScore", 0, 1);
    }

    public static void StartStage(StageInfo stage)
    {
        sm.CurrentStage = stage;
        sm.Record = stage.stageData.Score;
        sm.StageMode = true;
    }

    public static void StartInfinity()
    {
        sm.CheckInfinityRecord();
        sm.StageMode = false;
        sm.CurrentStage = null;
    }

    void StopGame()
    {
        CancelInvoke("AddScore");
        StagePassed();
    }

    void UpdateStageScore()
    {
        CurrentStage.stageData.Score = Score;
    }

    public void StagePassed()
    {
        if (Score > Record)
        {
            if (StageMode) UpdateStageScore();
            else SaveInfinityRecord();
        }
        if (StageMode && !PlayerManager.pm.Dead)
        GameManager.instance.Stopgame();
    }

    void SaveInfinityRecord()
    {
        PlayerPrefs.SetInt("Score", Score);
        CheckInfinityRecord();
    }

    void CheckInfinityRecord()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            Record = PlayerPrefs.GetInt("Score");
            InfRecordT.text = Record.ToString();
        }
        else
        {
            Record = 0;
            InfRecordT.text = "-";
        }
    }

    void AddScore()
    {
        ScoreT.text = (++Score).ToString();
        if (Score > Record && !NewRecord && Record != 0)
        {
            NewRecord = true;
            EventManager.OnNotification(new NotificationData("New Record", 60, Color.white));
        }

        if(!StageMode) EventManager.OnAddMoney((1, 0));
        EventManager.OnChangeScore(Score.ToString());
    }
}