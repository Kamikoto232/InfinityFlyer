using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Advertisements;
using System.Linq;

public class Results : MonoBehaviour, IUnityAdsListener
{
    public static Results r;
    public Section ResultsSection, DoubleSection;
    public TMP_Text TotalCreditsT, TotalTokensT, TotalScoreT;
    public TMP_Text DblCreditsT;
    public GameObject ADSButton, FinishT, GameOverT, FirstPassT, NewRecordT;
    int TotalCredits, TotalTokens, TotalExp;
    StageInfo CurrStage;
    Coroutine checker;
    List<ResultAction> resultActions = new List<ResultAction>();

    void Awake()
    {
        r = this;
    }

    void Start()
    {
        Advertisement.Initialize("3211315", false);
        Advertisement.AddListener(this);
        EventManager.StopGameHandler += OpenResultBoard;

        //EventManager.NearFlyHandler += () => AddAction("NearFly");
    }

    public static void StartStage(StageInfo stage)
    {
        r.CurrStage = stage;
        r.StartCollectInfo();
    }

    public static void StartInfinity()
    {
        r.CurrStage = null;
        r.StartCollectInfo();
    }

    void AddAction(string Name)
    {
        ResultAction ra = null;

        if (resultActions.Count > 0)
            ra = resultActions.Where(r => r.NameT.text == Name).First();

        if (ra)
        {
            ra.Count++;
        }
        else
        {
            ra.Name = Name;
            ra.Count = 1;

            resultActions.Add(ra);
        }
    }

    private void OpenResultBoard()
    {
        EventManager.AddMoneyHandler -= ((int c, int t) m) => { TotalCredits += m.c; TotalTokens += m.t; TotalExp += m.c + (m.t * 10); };

        if (PlayerManager.pm.Dead && CurrStage)
        {
            EventManager.OnBuy((TotalCredits, TotalTokens));
            TotalCredits = (int)(TotalCredits * 0.5f);
            TotalTokens = 0;
            EventManager.OnAddMoney((TotalCredits, TotalTokens));
        }

        EventManager.OnAddExp(TotalExp);
        StagePassedCheck();
        ChangeHeaderText();
        SetTexts();
        ResultsSection.Open();

        if (checker != null) StopCoroutine(checker);
        checker = StartCoroutine(AdChecker());
        foreach (ResultAction r in resultActions) r.SetData();
    }

    void SetTexts()
    {
        TotalCreditsT.text = TotalCredits.ToString();
        DblCreditsT.text = (TotalCredits + TotalCredits).ToString();
        TotalTokensT.text = TotalTokens.ToString();
        TotalScoreT.text = ScoreManager.sm.Score.ToString();
    }

    void ChangeHeaderText()
    {
        GameOverT.SetActive(PlayerManager.pm.Dead);
        FinishT.SetActive(!PlayerManager.pm.Dead);
        NewRecordT.SetActive(ScoreManager.NewRecord);
    }

    void StagePassedCheck()
    {
        if (!CurrStage || PlayerManager.pm.Dead)
        {
            FirstPassT.SetActive(false);
            return;
        }

        bool firstPass = CurrStage.MinScore <= ScoreManager.sm.Score && !CurrStage.stageData.Passed;
        FirstPassT.SetActive(firstPass);

        if (firstPass)
        {
            CurrStage.stageData.Passed = firstPass;
            TotalCredits += 200;
        }
    }

    IEnumerator AdChecker()
    {
        ADSButton.SetActive(false);
        
        while (!Advertisement.IsReady("rewardedVideo"))
        {
            //print(Advertisement.IsReady("rewardedVideo"));
            yield return new WaitForSeconds(0.5f);
        }
        ADSButton.SetActive(true);
    }

    private void StartCollectInfo()
    {
        TotalTokens = 0;
        TotalCredits = 0;
        EventManager.AddMoneyHandler += ((int c, int t) m) => { TotalCredits += m.c; TotalTokens += m.t; TotalExp += m.c + (m.t * 10); };
    }

    public void GetDoublePrize()
    {
        if (Advertisement.IsReady("rewardedVideo"))
            Advertisement.Show("rewardedVideo");
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == "rewardedVideo" && TotalCredits < 100)
            ADSButton.SetActive(true);
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished)
        {
            EventManager.OnAddMoney((TotalCredits, 0));
            DoubleSection.Open();
        }  
    }

    private void OnApplicationQuit()
    {
        EventManager.OnAddMoney((-TotalCredits, -TotalTokens));
    }
}