using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    [Header("Gravity")]
    public float gravityScale = 3.0f;
    public float gravity = 9.81f;

    [Header("Camera")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateGravity(Vector3.down);
    }

    void Update()
    {
        bool cHeld = Input.GetKey(KeyCode.C);
        if (cHeld)
        {
            ChangeGravityDirection();
        }
    }

    void FixedUpdate()
    {
        if (gravityDirection != Vector3.zero)
        {
            UpdateGravity(gravityDirection);
            gravityDirection = Vector3.zero;
        }
    }

    private void ChangeGravityDirection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gravityDirection = new Vector3(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gravityDirection = new Vector3(0, -1, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gravityDirection = new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            gravityDirection = new Vector3(-1, 0, 0);
        }
    }

    private void UpdateGravity(Vector3 direction)
    {
        Vector3 newGravity = cameraTransform.rotation * direction * gravity * gravityScale;
        Physics.gravity = newGravity;
    }
}
