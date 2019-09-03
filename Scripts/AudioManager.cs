using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource FX, Music, Ship;
    public AudioClip CollisionTrashSound, SectionOpen, SectionClose, Dead, Startgame, NearFly, Buy, PickUp;
    public AudioClip[] InGame, InMenu;
    public AudioMixer mixer;
    public AudioMixerSnapshot DrownedSn, NormalSn, GameSn;
    AudioClip LastMenu, LastGame;
    public RectTransform[] Coloumns;
    float[] SpectrumData = new float[64];
    public int SampleNum, LerpSpeed;
    Coroutine rmm;

    void Awake()
    {
        Instance = this;
        EventManager.StartFrictionHandler += () => FX.Play();
        EventManager.EndFrictionHandler += () => FX.Pause();
        EventManager.CollisionTrashHandler += () => Impact();
        EventManager.OpenSectionHandler += (s) => { if (s.CustomOpenSound) FX.PlayOneShot(s.CustomOpenSound); else FX.PlayOneShot(SectionOpen);  };
        EventManager.CloseSectionHandler += (s) => { FX.PlayOneShot(SectionClose); };
        EventManager.StopGameHandler += () => { StopGame(); };
        EventManager.StartGameHandler += () => { StartGame(); };
        EventManager.NearFlyHandler += () => {  FX.PlayOneShot(NearFly); };
        EventManager.GetShipHandler += (s) => { FX.PlayOneShot(Buy); };
        EventManager.PickUpHandler += (i) => { FX.PlayOneShot(PickUp); };
        EventManager.PauseGameHandler += Drowned;

        RandomMusic();
    }

    public void Impact()
    {
        FX.PlayOneShot(CollisionTrashSound);
    }

    public void RandomMusic()
    {
        CancelInvoke("RandomMenuMusic");
        if (rmm != null) StopCoroutine(rmm);
        rmm = StartCoroutine(RMM());
    }

    IEnumerator RMM()
    {
        while (Music.volume > 0.001)
        {
            Music.volume = Mathf.Lerp(Music.volume, 0, 4 * Time.deltaTime);
            yield return null;
        }
        Music.Stop();

        if (GameManager.instance.InGame)
        {
            while (Music.clip == LastGame)
            {

                LastGame = Music.clip;
                Music.clip = InGame[Random.Range(0, InGame.Length)];
                yield return null;
            }
        }
        else
        {
            while (Music.clip == LastMenu)
            {
                LastMenu = Music.clip;
                Music.clip = InMenu[Random.Range(0, InMenu.Length)];
                yield return null;
            }
        }
        
        Music.volume = 1;
        Invoke("RandomMusic", Music.clip.length);
        Music.Play();
    }

    void Drowned(bool enable)
    {
        if(enable)
        DrownedSn.TransitionTo(1f);
        else GameSn.TransitionTo(1f);
    }

    void StartGame()
    {
        FX.PlayOneShot(Startgame);
        GameSn.TransitionTo(.5f);
        RandomMusic();
    }

    void StopGame()
    {
        FX.PlayOneShot(Dead); FX.Stop();
        NormalSn.TransitionTo(.5f);
        RandomMusic();
    }

    void Update()
    {
        Music.GetSpectrumData(SpectrumData, 0, FFTWindow.Rectangular);
        if (!GameManager.instance.InGame)
        {
            MakeFreqBands();
            return;
        }
        EventManager.OnMusicPulse(SpectrumData[SampleNum]);
    }

    void MakeFreqBands()
    {
        for (int i = 0; i <= Coloumns.Length -1; i++)
        {
            Coloumns[i].localScale = Vector3.Lerp(Coloumns[i].localScale, new Vector3(1, Mathf.Clamp01(SpectrumData[i] * 2), 1), Time.deltaTime * LerpSpeed);
        }
    }
}