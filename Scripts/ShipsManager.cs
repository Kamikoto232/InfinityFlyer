using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager sm;
    public List<ShipData> Ships = new List<ShipData>();
    public int CurrIndex;
    public Transform ShipSpawnTr;
    public TMP_Text ShipNameText, CreditsCost, TokensCost, StrengthT, ControllabilityT, SpeedT, MassT, LimitedOfferTime;
    public Text t;
    public Slider StrengthBG, Strength, ControllabilityBG, Controllability, SpeedBG, Speed, Mass;
    public Button Select, Buy, Selected, Upgrade, Angar;
    public RawImage SpecBG;
    public GameObject CurrShip, CreditIcon, TokenIcon, LockIcon, LimitedOfferBanner, LimitedOfferMenuBanner, LimitedOfferText, ExpiredText, RecommendedBanner;
    public Section TriggerSection, MenuSection;
    public Section[] IgnoredSections;
    bool returnShip, SelectMode;
    int SelectedIndex;
    Coroutine UpdSl;
    ShipData RecommShip;
    ShipData[] OfferShips;

    void Awake()
    {
        sm = this;
    }

    private void Start()
    {
        EventManager.OpenSectionHandler += (s) =>
        {
            if (s == MenuSection) ShowRandomOfferShip();
            if (s == TriggerSection) { returnShip = true; Angar.interactable = false; }
            else
            {
                if (IgnoredSections.Any(se => se==s))
                {
                    Angar.interactable = false;
                    return;
                }
                if (returnShip) { Angar.interactable = true; returnShip = false; }
            }
        };
        EventManager.StartGameHandler += () => CurrShip.GetComponent<ShipModel>().Effects.SetActive(true);
        EventManager.DeadHandler += () => CurrShip.GetComponent<ShipModel>().Effects.SetActive(false);

        InitSliders();
        LoadData();
        OfferShips = Ships.Where(s => s.EndDate != Vector3Int.zero && !s.data.Buy).ToArray();
    }

    public void NextShip()
    {
        if (CurrIndex < Ships.Count - 1) ShowShip(Ships[++CurrIndex]);
        else ShowShip(Ships[CurrIndex = 0]);
    }

    public void PrevShip()
    {
        if (CurrIndex > 0) ShowShip(Ships[--CurrIndex]);
        else ShowShip(Ships[CurrIndex = Ships.Count - 1]);
    }

    void SetBannerAndExpiredTime(ShipData shipData)
    {
        Vector3Int endDate = shipData.EndDate;
        bool limited = shipData.EndDate != Vector3Int.zero && !shipData.data.Buy;
        bool expired;
        DateTime EndDate = DateTime.Now;
        SpecBG.color = (shipData.EndDate != Vector3Int.zero) ? Color.red : Color.black;

        if (limited)
            EndDate = new DateTime(endDate.z, endDate.y, endDate.x);
        LOBActivate(limited);
        if (!limited) return;

        expired = EndDate < DateTime.Now && !shipData.data.Buy;
        
        if (expired) DeactivateButtons();
        else
        {
            string date = string.Empty;
            date += (endDate.x < 9) ? $"0{endDate.x}" : endDate.x.ToString();
            date += "/" + ((endDate.y < 9) ? $"0{endDate.y}" : endDate.y.ToString()) + "/";
            date += endDate.z.ToString();
            LimitedOfferTime.text = date;
        }
        LimitedOfferText.SetActive(!expired);
        ExpiredText.SetActive(expired);
    }

    void ShowRandomOfferShip()
    {

        if (OfferShips.Length == 0 || UnityEngine.Random.Range(0, 100) < 38)
        {
            if (CurrIndex == SelectedIndex) return;
            CameraManager.RandomMenuAnimation();
            ShowLastSelectedShip();
            return;
        }

        int randIndex = UnityEngine.Random.Range(0, OfferShips.Length);
        if (Ships.IndexOf(OfferShips[randIndex]) != CurrIndex) CameraManager.RandomMenuAnimation();
        ShowShip(OfferShips[randIndex]);
    }

    public static void ShowShip(ShipData shipData)
    {
        sm.CurrIndex = sm.Ships.IndexOf(shipData);
        sm.ShipNameText.text = shipData.name;
        Destroy(sm.CurrShip);
        sm.CurrShip = Instantiate(shipData.Model, sm.ShipSpawnTr).gameObject;
        sm.CurrShip.GetComponent<ShipModel>().Effects.SetActive(false);
        Controller.rb.mass = shipData.Mass;

        sm.UpdateSliders(shipData);
        sm.BuyButtonsChange(shipData.data.Buy);
        sm.SetBannerAndExpiredTime(shipData);
        if(sm.RecommShip)
        sm.ShowBannerRecommended(sm.RecommShip.name == shipData.name);
        else sm.ShowBannerRecommended(false);
    }

    public static void ShowRecommendedShip(ShipData shipData)
    {
        sm.RecommShip = shipData;
        ShowShip(shipData);
    }

    public static void ShowLastSelectedShip()
    {
        sm.RecommShip = null;
        ShowShip(sm.Ships[sm.SelectedIndex]);
    }

    public static void SetSelectShipMode()
    {
       sm.SelectMode = true;
    }

    void ShowBannerRecommended(bool show)
    {
        RecommendedBanner.SetActive(show);
    }

    void UpdateSliders(ShipData shipData)
    {
        StrengthT.text = shipData.shipLevels[shipData.data.HealthLevel].Health.ToString();
        ControllabilityT.text = shipData.shipLevels[shipData.data.ControllabilityLevel].Controllability.ToString();
        SpeedT.text = shipData.Speed.ToString();
        MassT.text = shipData.Mass.ToString();

        if (UpdSl != null) StopCoroutine(UpdSl);
        UpdSl = StartCoroutine(UpdaterSlider(shipData.shipLevels[4].Health, 
            shipData.shipLevels[shipData.data.HealthLevel].Health,
            shipData.shipLevels[shipData.data.ControllabilityLevel].Controllability,
            shipData.shipLevels[4].Controllability, shipData.Speed, shipData.Speed, shipData.Mass));
    }

    IEnumerator UpdaterSlider(float SBG, float S, float C, float CBG, float Sp, float SpBG, float mass)
    {
        float t = 3f;
        while(t > 0)
        {
            StrengthBG.value = Mathf.Lerp(StrengthBG.value, SBG, Time.deltaTime * 20);
            Strength.value = Mathf.Lerp(Strength.value, S, Time.deltaTime * 20);
            Controllability.value = Mathf.Lerp(Controllability.value, C, Time.deltaTime * 20);
            ControllabilityBG.value = Mathf.Lerp(ControllabilityBG.value, CBG, Time.deltaTime * 20);
            Speed.value = Mathf.Lerp(Speed.value, Sp, Time.deltaTime * 20);
            SpeedBG.value = Mathf.Lerp(SpeedBG.value, SpBG, Time.deltaTime * 20);
            Mass.value = Mathf.Lerp(Mass.value, mass, Time.deltaTime * 20);


            t -= Time.deltaTime;
            yield return null;
        }
    }

    void InitSliders()
    {
        float MaxStr = 0;
        float MaxContrl = 0;
        float MaxSpeed = 0;
        float MaxMass = 0;

        foreach(ShipData s in Ships)
        {
            if (s.shipLevels[4].Health > MaxStr) MaxStr = s.shipLevels[4].Health;
            if (s.shipLevels[4].Controllability > MaxContrl) MaxContrl = s.shipLevels[4].Controllability;
            if (s.Speed > MaxSpeed) MaxSpeed = s.Speed;
            if (s.Mass > MaxMass) MaxMass = s.Mass;
        }

        StrengthBG.maxValue = MaxStr;
        Strength.maxValue = MaxStr;
        ControllabilityBG.maxValue = MaxContrl;
        Controllability.maxValue = MaxContrl;
        SpeedBG.maxValue = MaxSpeed;
        Speed.maxValue = MaxSpeed;
        Mass.maxValue = MaxMass;
    }

    void BuyButtonsChange(bool buy)
    {
        t.text = "buy=" + buy.ToString() + " sm=" + SelectMode.ToString();
        SetCost((Ships[CurrIndex].Price, Ships[CurrIndex].TokenPrice));
        Buy.gameObject.SetActive(!buy);
        Select.gameObject.SetActive(buy && SelectMode);
        Selected.gameObject.SetActive(false);
        Upgrade.gameObject.SetActive(buy);
    }

    void DeactivateButtons()
    {
        Buy.gameObject.SetActive(false);
        Select.gameObject.SetActive(false);
        Selected.gameObject.SetActive(false);
        Upgrade.gameObject.SetActive(false);
    }

    void LOBActivate(bool activate)
    {
        LimitedOfferBanner.SetActive(activate);
        LimitedOfferMenuBanner.SetActive(activate);
    }

    void SetCost((int c, int t) cost)
    {
        CreditIcon.SetActive(true);
        TokenIcon.SetActive(cost.t > 0);
        CreditsCost.text = cost.c.ToString();
        TokensCost.text = cost.t.ToString();
    }

    public void SelectCurrentShip()
    {
        EventManager.OnChangeShip(Ships[CurrIndex]);
        SelectedIndex = CurrIndex;
        SetSelected();
        RecommShip = null;
        SaveData();
    }

    void SetSelected()
    {
        Selected.gameObject.SetActive(SelectMode);
        Upgrade.gameObject.SetActive(true);
        Buy.gameObject.SetActive(false);
        Select.gameObject.SetActive(false);
        sm.SelectMode = false;
        //LockIcon.SetActive(false);
    }

    public void BuyCurrentShip()
    {
        bool Succes = EventManager.OnBuy((Ships[CurrIndex].Price, Ships[CurrIndex].TokenPrice));
        if (Succes)
        {
            AddShip(Ships[CurrIndex]);
            SelectCurrentShip();
        }
    }

    public void AddShip(ShipData ship)
    {
        EventManager.OnGetShip(ship);
        ship.data.Buy = true;
        BuyButtonsChange(true);
        SaveData();
        StartCoroutine(HideTimer());
    }

    IEnumerator HideTimer()
    {
        Header.h.HideSection();
        yield return new WaitForSecondsRealtime(4f);
        Header.h.ShowSection();
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/Sdata";
        string Data = string.Empty;

        foreach(ShipData sd in Ships)
        {
            Data += JsonUtility.ToJson(sd.data);
            Data += "#";
        }

        Data += SelectedIndex.ToString();
        File.WriteAllText(path, Data);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/Sdata";
        print(path);
        if (!File.Exists(path))
        {
            AddShip(Ships[0]);
        }

        string Data = File.ReadAllText(path);
        string[] Datas = Data.Split('#');
        
        for (int i = 0; i < Ships.Count + 1; i++) 
        {
            if (int.TryParse(Datas[i], out SelectedIndex)) break;
            if (i > Datas.Length - 1) continue;
            if (i > Ships.Count - 1) continue;
            Ships[i].data = JsonUtility.FromJson<Data>(Datas[i]);
        }
        ShowShip(Ships[SelectedIndex]);
        SelectCurrentShip();
    }

    private void OnApplicationQuit()
    {
        SaveData();
        foreach (ShipData sd in Ships) sd.data.Buy = false;
    }
}