using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    private Rigidbody rb;
    public Transform cameraTransform; 

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 gravityDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravityDirection = new Vector3(0, 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityDirection = new Vector3(0, -1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravityDirection = new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gravityDirection = new Vector3(-1, 0, 0);
        }

        // Apply the camera's rotation to the gravity direction
        if (gravityDirection != Vector3.zero)
        {
            Vector3 transformedGravityDirection = cameraTransform.rotation * gravityDirection * 9.81f;
            Physics.gravity = transformedGravityDirection;
        }
    }
}
