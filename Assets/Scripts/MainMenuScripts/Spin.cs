using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spin : MonoBehaviour
{
    float r;
    public float rot = 0f;

    void Update()
    {
        rot = transform.eulerAngles.y + 1.5f;
        float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rot, ref r, 0.1f);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Angle, transform.eulerAngles.z);
    }
}
