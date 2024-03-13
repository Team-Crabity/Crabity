using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float groundDrag;

    private Rigidbody rb;
    private SwitchGravity switchGravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        switchGravity = GetComponent<SwitchGravity>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 gravityDirection = switchGravity.gravityDirection; // Get the current gravity direction
        Vector3 movementDirection = Vector3.zero;

        // Adjust movement direction based on gravity direction
        if (gravityDirection == Vector3.left || gravityDirection == Vector3.right)
        {
            // Move up or down relative to the camera's orientation when gravity is left or right
            // Vector3 cameraForward = Camera.main.transform.forward;
            // cameraForward.x = 0; //Prevent player from moving left or right
            // movementDirection = cameraForward.normalized * verticalInput;
            movementDirection = verticalInput * Vector3.up;
        }
        else
        {
            // Normal movement on the ground
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0; // Prevents the player from moving up or down
            movementDirection = cameraRight.normalized * horizontalInput;
        }

        float effectiveSpeed = speed * (1 - groundDrag * Time.deltaTime);
        rb.MovePosition(rb.position + movementDirection * effectiveSpeed * Time.deltaTime);
    }
}
