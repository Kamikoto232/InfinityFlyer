using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager mm;
    public MoneyData moneyData;
    public Section NoMoneyWarn;

    public TMP_Text CreditsText, TokensText;

    private void Awake()
    {
        mm = this;
    }

    void Start()
    {
        moneyData = new MoneyData();
        EventManager.BuyHandler += Buy;
        EventManager.AddMoneyHandler += Add;
        EventManager.NearFlyHandler += () => Add((0,1));
        EventManager.PostStopGameHandler += () => SaveData();
        LoadData();
    }

    bool Buy((int Credits,int Tokens) Price)
    {
        if (moneyData.Credits - Price.Credits > 0 && moneyData.Tokens - Price.Tokens > 0)
        {
            moneyData.Credits -= Price.Credits;
            moneyData.Tokens -= Price.Tokens;
            UpdateValues();
            SaveData();
            return true;
        }
        else
        {
            NoMoneyWarn.Open();
            return false;
        }
    }

    void Add((int c, int t) value)
    {
        moneyData.Credits += value.c;
        moneyData.Tokens += value.t;
        UpdateValues();
        SaveData();
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/Mdata";
        string Data = JsonUtility.ToJson(moneyData);
        File.WriteAllText(path, Data);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/Mdata";
        if (!File.Exists(path))
        {
            moneyData.Credits = 100;
            moneyData.Tokens = 5;
            SaveData();
        }
        else
        {
            string Data = File.ReadAllText(path);
            moneyData = JsonUtility.FromJson<MoneyData>(Data);
        }
        UpdateValues();
    }

    void UpdateValues()
    {
        TokensText.text = moneyData.Tokens.ToString();
        CreditsText.text = moneyData.Credits.ToString();
        if (CreditsText.text.Length > 3)
            CreditsText.text.Insert(CreditsText.text.Length - 2, ",");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

[System.Serializable]
public struct MoneyData
{
    public int Credits;
    //{
    //    get { return Credits; }
    //    set
    //    {
    //        MoneyManager.mm.CreditsText.text = value.ToString();
    //        if (MoneyManager.mm.CreditsText.text.Length > 3)
    //            MoneyManager.mm.CreditsText.text.Insert(MoneyManager.mm.CreditsText.text.Length - 2, ",");
    //        Credits = value;
    //    }
    //}
    public int Tokens;//{ get { return Tokens; } set { MoneyManager.mm.TokensText.text = value.ToString(); Tokens = value; } }

    public MoneyData(int credits, int tokens)
    {
        Credits = credits;
        Tokens = tokens;
    }
}