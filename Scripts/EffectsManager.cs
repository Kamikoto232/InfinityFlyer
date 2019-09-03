using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager em;
    public GameObject Sparkle;
    public GameObject Boom;

    void Awake()
    {
        em = this;
    }
}
