using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager sm;
    StageInfo stage;
    public Section Angar;
    public StageInfo[] Stages;
    public Transform StagesRoot;
    public GameObject StageBtn;

    private void Awake()
    {
        sm = this;
    }

    private void Start()
    {
        GenerateStageButtons();
    }

    void StartGame()
    {
        EventManager.OnChangeColor(stage.color);
        SpawnManager.StartStageSpawnMap(stage);
        GameManager.instance.Startgame();
        ScoreManager.StartStage(stage);
        Results.StartStage(stage);
        EventManager.ChangeShipHandler -= (s) => StartGame();
        stage = null;
    }

    void GenerateStageButtons()
    {
        foreach(StageInfo s in Stages)
        {
            StageButton sb = Instantiate(StageBtn, StagesRoot).GetComponent<StageButton>();
            sb.SetData(s);
        }
    }

    public void SetMode(StageInfo st)
    {
        stage = st;
        EventManager.ChangeShipHandler += (s) => StartGame();
        Angar.Open();
        ShipsManager.SetSelectShipMode();
        if (st.RecommendedShip) ShipsManager.ShowRecommendedShip(st.RecommendedShip);
        else ShipsManager.ShowLastSelectedShip();
    }

    public void CancelMode()
    {
        EventManager.ChangeShipHandler -= (s) => StartGame();
    }

    public static void SelectStage(StageInfo st)
    {
        StageInfoWindow.ShowStage(st);
    }
}