using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherLerp : MonoBehaviour
{
    public float Speed;
    public Transform Target;
    public Vector3 Offset;

    Vector3 newPos;
    Transform tr;

    void Start()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = Vector3.Lerp(tr.position,  new Vector3(0,0,Target.position.z) + Offset, Speed * Time.deltaTime);

        tr.position = Vector3.Lerp(tr.position, newPos, Speed * Time.deltaTime);
    }
}
