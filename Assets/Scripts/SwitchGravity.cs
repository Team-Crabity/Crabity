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

    [Header("Movement")]
    public float speed = 7.0f;
    public float groundDrag = 0.5f;

    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.zero;
    private bool gravityCooldown;

    // Track the current orientation of gravity to adjust movement
    private Vector3 currentGravityDirection = Vector3.down;

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
            currentGravityDirection = gravityDirection; // Update current gravity direction
            gravityDirection = Vector3.zero;
        }
        MovePlayer();
    }

    private void ChangeGravityDirection()
    {
        Dictionary<KeyCode, Vector3> keyToDirection = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, Vector3.up },
            { KeyCode.S, Vector3.down },
            { KeyCode.D, Vector3.right },
            { KeyCode.A, Vector3.left }
        };

        foreach (var keyDirectionPair in keyToDirection)
        {
            if (Input.GetKeyDown(keyDirectionPair.Key) && gravityCooldown)
            {
                gravityDirection = keyDirectionPair.Value;
                gravityCooldown = false;
                break;
            }
        }
    }

    private void UpdateGravity(Vector3 direction)
    {
        Vector3 newGravity = cameraTransform.rotation * direction * gravity * gravityScale;
        Physics.gravity = newGravity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            gravityCooldown = true;
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = Vector3.zero;

        if (currentGravityDirection == Vector3.down || currentGravityDirection == Vector3.up)
        {
            // Gravity is down/up, player moves left and right with A and D keys
            movementDirection += cameraTransform.right * horizontalInput;
        }
        else if (currentGravityDirection == Vector3.left || currentGravityDirection == Vector3.right)
        {
            // Gravity is left/right, player moves up and down with W and S keys
            movementDirection += cameraTransform.up * verticalInput;
        }

        // Ensure movementDirection does not include movement along the camera's forward axis
        movementDirection = Vector3.ProjectOnPlane(movementDirection, cameraTransform.forward);

        float effectiveSpeed = speed * (1 - groundDrag * Time.deltaTime);
        rb.MovePosition(rb.position + movementDirection.normalized * speed * Time.deltaTime);
    }
}
