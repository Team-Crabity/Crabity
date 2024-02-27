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
        bool ctrlHeld = Input.GetKey(KeyCode.LeftControl);

        Vector3 gravityDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W) && shiftHeld)
        {
            gravityDirection = new Vector3(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.S) && shiftHeld)
        {
            gravityDirection = new Vector3(0, -1, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && shiftHeld)
        {
            gravityDirection = new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A) && shiftHeld)
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
