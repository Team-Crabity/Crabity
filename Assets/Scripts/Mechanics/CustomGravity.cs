using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public Vector3 localGravityDirection = Vector3.down;
    public float gravity = 9.81f;
    public float gravityScale = 5.0f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 gravityForce = localGravityDirection.normalized * gravity * gravityScale;
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }
}