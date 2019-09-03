using Kino;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cm;
    public Camera cam;
    public AnalogGlitch analogGlitch;
    public DigitalGlitch digitalGlitch;
    public Animation anim;
    public static bool OrbitMode;
    public Section TrigOrbitSect;

    private void Awake()
    {
        cm = this;
    }

    void Start()
    {
        EventManager.DamageHandler += (dmg) =>
        {
            anim["Damage"].speed = FPSSpeedData.AnimationSpeed;

            anim.Play("Damage");
        };
        EventManager.DeadHandler += () =>
        {
            anim["Dead"].speed = FPSSpeedData.AnimationSpeed;

            anim.Play("Dead");
        };
        EventManager.StartGameHandler += StartGame;
        EventManager.PostStopGameHandler += PostStopGame;
        EventManager.OpenSectionHandler += (s) =>
        {
            if (GameManager.instance.InGame) return;
            if (s == TrigOrbitSect && OrbitMode) ChangeOrbitMode();

            if (s.CameraPoint) anim.Play("StartGame", PlayMode.StopAll);
            else
            {
                if (cm.anim.IsPlaying("Default")) return;
                RandomMenuAnimation();
            }
        };

        EventManager.GetShipHandler += (s) =>
        {
            bool def = anim.IsPlaying("Default");
            anim.Play("GetShip", PlayMode.StopAll);
            if (def) anim.PlayQueued("Default");
        };
    }
    private void StartGame()
    {
        anim.Play("StartGame", PlayMode.StopAll);
    }

    private void PostStopGame()
    {
        analogGlitch.enabled = false;
        digitalGlitch.enabled = false;
        analogGlitch.verticalJump = 0;
        anim.Play("Default");
    }

    public void ChangeOrbitMode()
    {
        OrbitMode = !OrbitMode;
    }

    public static void RandomMenuAnimation()
    {
        //if (cm.anim.IsPlaying("Default")) return;
        cm.anim.Stop();
        cm.anim.Play("Default");
        cm.anim["Default"].speed = FPSSpeedData.AnimationSpeed;
        switch (Random.Range(0, 5))
        {
            case 0:
                cm.anim["Default"].time = 0;
                break;

            case 1:
                cm.anim["Default"].time = 5;
                break;

            case 2:
                cm.anim["Default"].time = 9.3f;
                break;

            case 3:
                cm.anim["Default"].time = 15;
                break;

            case 4:
                cm.anim["Default"].time = 19.3f;
                break;

            case 5:
                cm.anim["Default"].time = 23;
                break;
        }
    }
}