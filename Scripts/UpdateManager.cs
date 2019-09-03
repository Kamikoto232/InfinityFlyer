using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;
    List<Action> Updates = new List<Action>();
    List<Action> FixedUpdates = new List<Action>();
    List<Action> LateUpdates = new List<Action>();
    int Updcount;
    bool First;
    bool FFirst;
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Updcount = Updates.Count;
         for (int i = 0; i < Updcount; i++) Updates[i].Invoke();
    }

    void FixedUpdate()
    {
        int count = FixedUpdates.Count;
         for (int i = 0; i < count; i++) FixedUpdates[i].Invoke();
    }

    private void LateUpdate()
    {
        int count = LateUpdates.Count;
        for (int i = 0; i < count; i++) LateUpdates[i].Invoke();
    }

    public void RegisterUpdate(Action action)
    {
        Updates.Add(action);
    }

    public void UnregisterUpdate(Action action)
    {
        Updates.Remove(action);
    }

    public void RegisterLUpdate(Action action)
    {
        LateUpdates.Add(action);
    }

    public void UnregisterLUpdate(Action action)
    {
        LateUpdates.Remove(action);
    }

    public void RegisterFUpdate(Action action)
    {
        FixedUpdates.Add(action);
    }

    public void UnregisterFUpdate(Action action)
    {
        FixedUpdates.Remove(action);
    }
}