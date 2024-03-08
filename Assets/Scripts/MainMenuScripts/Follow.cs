using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject Target;
    float dist;

    void Start()
    {
        dist = transform.position.y - Target.transform.position.y;
    }
    void Update()
    {
        
        transform.position = new Vector3 (transform.position.x, Target.transform.position.y + dist,  transform.position.z);
    }
}
