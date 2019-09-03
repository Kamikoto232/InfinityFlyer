using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    //Animator anim;
   // Transform tr;


    private void OnEnable()
    {
        EventManager.PostStopGameHandler += Delete;
    }

    private void OnDisable()
    {
        //UpdateManager.Instance.UnregisterUpdate(Updated);
        //UpdateManager.Instance.UnregisterFUpdate(FixedUpdated);

        EventManager.PostStopGameHandler -= Delete;
    }

    void Delete()
    {
        Destroy(gameObject);
    }
}
