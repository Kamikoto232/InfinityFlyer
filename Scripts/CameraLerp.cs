using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public static CameraLerp cl;
    public float Speed;
    public Vector3 Offset;
    public Transform Target;
    Transform tr, MainPoint;
    Vector3 newPos, Initmousepos, LastAxis;
    Quaternion newRot = Quaternion.identity;
    float t = 0.05f, accel;
    bool lerp;
    Coroutine timer;

    private void Awake()
    {
        cl = this;
    }

    void Start()
    {
        MainPoint = Target;
        EventManager.StartGameHandler += StartGame;
        EventManager.PostStopGameHandler += PostStopGame;
        EventManager.OpenSectionHandler += (s) => { if (s.CameraPoint && !GameManager.instance.InGame) LerpToPoint(s.CameraPoint); };
        EventManager.ShowSectionHandler += (s) => { if (s.CameraPoint && !GameManager.instance.InGame) LerpToPoint(s.CameraPoint); };

        tr = transform;

        enabled = false;

        StartCoroutine(PosUpdater());
    }

    private void StartGame()
    {
        LerpToPoint(MainPoint);

    }

    void Update()
    {
        if (CameraManager.OrbitMode)
        {
            if (Input.GetMouseButtonDown(0)) Initmousepos.Set(0, -Input.mousePosition.x, 0);
            Vector3 newmp = Vector3.zero;
            if (Input.GetMouseButton(0))
            {
                if (t < 0)
                {
                    Initmousepos.Set(0, -Input.mousePosition.x, 0);
                    t = 0.05f;
                }
                else t -= Time.deltaTime;
                newmp = new Vector3(0, -Input.mousePosition.x, 0);
                LastAxis = Initmousepos - newmp;
            }

            if (Input.GetMouseButtonUp(0)) Initmousepos = Vector3.zero;

            accel = Mathf.Clamp(Mathf.Lerp(accel, (Initmousepos - newmp).sqrMagnitude / 200, Time.deltaTime), 0, 10);
            tr.RotateAround(Target.position, LastAxis, accel);
            return;
        }
        else accel = 0;

        if (PlayerManager.pm.Dead)
        { 
            tr.LookAt(Target);
        }
        
        
            newPos = Vector3.Lerp(transform.position, Target.position, Speed + Time.deltaTime * 10);
            newRot = Quaternion.Lerp(transform.rotation, Target.rotation, Speed + Time.deltaTime * 10);

            if (PlayerManager.pm.Dead || CameraManager.OrbitMode) return;
            tr.position = newPos;
            tr.rotation = newRot;
    }

   public static void LerpToPoint(Transform point)
    {
        //if (tr.position.Equals(point.position)) return;
        cl.newPos = cl.transform.position;
        cl.newRot = cl.transform.localRotation;
        cl.lerp = true;
        cl.Target = point;
        cl.Speed = 0;
        if(cl.timer != null)  cl.StopCoroutine(cl.timer);
        cl.timer = cl.StartCoroutine(cl.LerpTimer());
    }

    IEnumerator LerpTimer()
    {
        lerp = false;
        cl.newPos = cl.transform.position;
        cl.newRot = cl.transform.localRotation;
        enabled = true;

        yield return new WaitForSecondsRealtime(2f);
        
        enabled = !GameManager.instance.InGame;
        enabled = !CameraManager.OrbitMode;
    }

    IEnumerator PosUpdater()
    {
        WaitForSecondsRealtime w = new WaitForSecondsRealtime(0.5f);
        while (true)
        {
            yield return w;
            if (lerp) continue;
            cl.newPos = cl.transform.position;
            cl.newRot = cl.transform.localRotation;
        }
    }

    void PostStopGame()
    {
        enabled = false;
        newPos = tr.position;
        newRot = tr.rotation;
    }
}