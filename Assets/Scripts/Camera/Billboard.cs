using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
