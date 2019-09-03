using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats ps;
    public static string Name;
    public PlayerData stats;
    public TMP_Text LevelT, TotalExpT, NeedExpT, NameT;
    public Slider ExpSlider;
    public Section RegistrationSect, UpdateUISection;
    int DisplayerTotalExp, DisplayerNeedExp;
    bool FirstPlay;
    private void Awake()
    {
        ps = this;
    }

    void Start()
    {
        FirstPlay = true;
        EventManager.AddExpHandler += AddExp;
        LoadData();
    }

    void AddExp(int exp)
    {
        stats.TotalExp += exp;

        while (stats.TotalExp >= GetNeedExp())
        {
            stats.Level += 1;
            EventManager.OnLevelUp(stats.Level);
        }
        UpdateUI();
    }

    IEnumerator UpdateUIAnimated()
    {
        float t = 0;

        while(t < 10)
        {
            TotalExpT.text = (DisplayerTotalExp = (int)Mathf.Lerp(DisplayerTotalExp, stats.TotalExp, t)).ToString();
            NeedExpT.text = (DisplayerNeedExp = (int)Mathf.Lerp(DisplayerNeedExp, GetNeedExp(), t)).ToString();
            ExpSlider.maxValue = Mathf.Lerp(ExpSlider.maxValue, GetNeedExp(), t);
            ExpSlider.minValue = Mathf.Lerp(ExpSlider.minValue, GetMinExp(), t);
            ExpSlider.value = Mathf.Lerp(ExpSlider.value, stats.TotalExp, t);
            t += Time.deltaTime;
            yield return null;
        }

        if(!FirstPlay) EventManager.OpenSectionHandler -= UUIAStarter;
        FirstPlay = false;
    }

    void UpdateUI()
    {
        NameT.text = stats.Name;
        LevelT.text = stats.Level.ToString();
        //ExpSlider.maxValue = GetNeedExp();
        //ExpSlider.minValue = GetMinExp();
        //ExpSlider.value = stats.TotalExp;
        if (!FirstPlay) EventManager.OpenSectionHandler += UUIAStarter;
        else UUIAStarter(UpdateUISection);
        SaveData();
    }

    void UUIAStarter(Section s)
    {
        if (s != UpdateUISection) return;
        StopAllCoroutines();
        StartCoroutine(UpdateUIAnimated());
    }

    int GetNeedExp()
    {
        return stats.Level * 50;
    }

    int GetMinExp()
    {
        return (stats.Level - 1) * 50;
    }

    void SaveData()
    {
        string data;
        string path = Application.persistentDataPath + "/Pdata";

        data = JsonUtility.ToJson(stats);
        File.WriteAllText(path, data);
    }

    void LoadData()
    {
        string path = Application.persistentDataPath + "/Pdata";
        string data;

        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
            stats = JsonUtility.FromJson<PlayerData>(data);
        }
        else ShowRegistrationSection();
        UpdateUI();
    }

    void ShowRegistrationSection()
    {
        RegistrationSect.Open();
    }

    public static void SetPlayerNameAndAge(string name, string age)
    {
        ps.RegistrationSect.Close();
        ps.stats.Name = name;
        ps.stats.Age = int.Parse(age);
        ps.stats.Level = 1;
        ps.stats.TotalExp = 0;
        ps.UpdateUI();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

[System.Serializable]
public struct PlayerData
{
    public string Name;
    public int Age, Level;
    public int TotalExp;
}