using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FxProNS;

public class SettingManager : MonoBehaviour
{
    public static SettingManager sm;
    public TMP_Text CurrFPS;
    public Toggle VLow, Low, Med, High;
    public Slider FpsSlider;
    public Material Particles, Lines, Lighted;
    public ReflectionProbe rp;
    FxPro fx;
    BloomHelperParams bloomHelperParams = new BloomHelperParams();
    int quality = 0;

    void Awake()
    {
        sm = this;
    }

    void Start()
    {
        fx = CameraManager.cm.analogGlitch.gameObject.GetComponent<FxPro>();

        if (PlayerPrefs.HasKey("Quality"))
        {
            FpsSlider.value = PlayerPrefs.GetFloat("FPS");
            ChangeFPS(FpsSlider.value);

            quality = PlayerPrefs.GetInt("Quality");
            ChangeSetting(quality);
        }
        else
        {
            ChangeFPS(FpsSlider.value);
            ChangeSetting(quality);
        }

        VLow.onValueChanged.AddListener((v) => { if (!v) return; quality = 0; ChangeSetting(quality); });
        Low.onValueChanged.AddListener((v) => { if (!v) return; quality = 1; ChangeSetting(quality); });
        Med.onValueChanged.AddListener((v) => { if (!v) return; quality = 2; ChangeSetting(quality); });
        High.onValueChanged.AddListener((v) => { if (!v) return; quality = 3; ChangeSetting(quality); });
        FpsSlider.onValueChanged.AddListener(ChangeFPS);
    }

    void ChangeSetting(int x)
    {
        switch (quality)
        {
            case 0:
                VLow.isOn = true;
                break;
            case 1:
                Low.isOn = true;
                break;
            case 2:
                Med.isOn = true;
                break;
            case 3:
                High.isOn = true;
                break;
        }

        QualitySettings.SetQualityLevel(x);
        PlayerPrefs.SetInt("Quality", x);

        fx.enabled = false;
        CameraManager.cm.cam.allowHDR = true;
        Emission(true);
        rp.gameObject.SetActive(true);
        bloomHelperParams.BloomIntensity = 0.65f;
        bloomHelperParams.BloomSoftness = 0.572f;

        switch (x)
        {
            case 0:
                CameraManager.cm.cam.allowHDR = false;
                Emission(false);
                rp.gameObject.SetActive(false);
                break;

            case 1:
                bloomHelperParams.Quality = FxProNS.EffectsQuality.Fastest;
                fx.enabled = true;
                rp.gameObject.SetActive(false);
                break;

            case 2:
                bloomHelperParams.Quality = FxProNS.EffectsQuality.Fast;
                fx.enabled = true;
                rp.resolution = 64;
                break;

            case 3:
                bloomHelperParams.Quality = FxProNS.EffectsQuality.Normal;
                fx.enabled = true;
                rp.resolution = 128;
                break;
        }

        BloomHelper.Instance.SetParams(bloomHelperParams);
        BloomHelper.Instance.Init();
    }

    void ChangeFPS(float fps)
    {
        CurrFPS.text = fps.ToString();
        PlayerPrefs.SetFloat("FPS", FpsSlider.value);
        Application.targetFrameRate = (int)FpsSlider.value;
    }

    void Emission(bool enabled)
    {
        if (enabled)
        {
            Particles.SetFloat("_Intens", 3);
            Lines.SetFloat("_Intens", 1);
            Lighted.SetFloat("_Intens", 2.5f);
        }
        else
        {
            Particles.SetFloat("_Intens", 1f);
            Lines.SetFloat("_Intens", 0.5f);
            Lighted.SetFloat("_Intens", 1f);
        }   
    }
}