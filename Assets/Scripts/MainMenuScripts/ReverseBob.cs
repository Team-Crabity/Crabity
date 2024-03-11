using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseBob : MonoBehaviour
{
    public float amp;
    public float Hz;
    Vector3 initPos;

    void Start()
    {
        initPos = transform.position;
    }
    void Update()
    {
        transform.position = new Vector3(initPos.x, -Mathf.Sin(Time.time * Hz) * amp + initPos.y, initPos.z);
    }
}
