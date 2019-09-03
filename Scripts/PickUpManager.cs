using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    void Start()
    {
        EventManager.PickUpHandler += PickUp;
    }

    void PickUp(Collectable c)
    {

    }
}
