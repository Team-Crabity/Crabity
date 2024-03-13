using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float groundDrag;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 movementDirection = Vector3.zero;

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0; // Prevents the player from moving up or down
        movementDirection = cameraRight.normalized * horizontalInput;


        float effectiveSpeed = speed * (1 - groundDrag * Time.deltaTime);
        rb.MovePosition(rb.position + movementDirection * effectiveSpeed * Time.deltaTime);
    }
}
