using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float groundDrag;

    // public Transform orientation;

    private void Update()
    {
        // Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red); // Visualize the raycast
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement direction relative to the camera's orientation

        // Gets the right vector of the camera
        Vector3 cameraRight = Camera.main.transform.right; 
        cameraRight.y = 0; // Prevents the player from moving up or down
        Vector3 movementDirection = cameraRight.normalized * horizontalInput;

        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }

    // private void SpeedControl()
    // {
    //     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //     // limit velocity if needed
    //     if (flatVel.magnitude > moveSpeed)
    //     {
    //         Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //         rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    //     }
    // }
}