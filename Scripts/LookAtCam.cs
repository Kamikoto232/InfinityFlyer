using System.Collections;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Iterator());
    }

    IEnumerator Iterator()
    {
        WaitForSecondsRealtime w = new WaitForSecondsRealtime(0.11f);
        while (true)
        {
            transform.LookAt(CameraManager.cm.cam.transform);
            yield return w;
        }
    }
}