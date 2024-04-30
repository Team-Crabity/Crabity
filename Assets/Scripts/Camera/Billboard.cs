using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;
    public Transform follow;
    public bool isPlayer = false;
    
    private void LateUpdate()
    {
        if (isPlayer)
        {
                transform.position = follow.position + new Vector3(0, 5, 0);
        }
        transform.LookAt(transform.position + cam.forward);
    }
}