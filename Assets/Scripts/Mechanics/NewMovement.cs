using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    [Header("Player Selection")]
    public bool playerOne;
    public bool playerTwo;

    [Header("Gravity")]
    public float gravityScale = 5.0f;
    public float gravity = 9.81f;

    [Header("Movement")]
    [Range(5f, 20f)] public float speed = 7.0f;
    [Range(0f, 0.5f)] public float groundDrag = 0.5f;
    // [Range(0f, 0.5f)] public float airDrag = 0.25f; //Can add different drag cofeffcient when the player is in the air for faster speed

    [Header("Jumping")]
    public int jumpCounter = 1;
    public float jumpMultiplier;
    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }

    [Header("Animator")]
    public Animator animator;

    private Rigidbody rb;
    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;
    private Vector3 gravityDirection = Vector3.zero;
    public Vector3 newGravity { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        UpdateGravity(Vector3.down);

        //Set keybinds based on player selection
        if (playerOne)
        {
            up = KeyCode.W;
            down = KeyCode.S;
            left = KeyCode.A;
            right = KeyCode.D;
        }
        else
        {
            up = KeyCode.UpArrow;
            down = KeyCode.DownArrow;
            left = KeyCode.LeftArrow;
            right = KeyCode.RightArrow;
        }
    }

    void Update()
    {
        // Check if player is on mac
        bool isOnMac = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX;
        bool rightHeld = isOnMac ? Input.GetKey(KeyCode.RightAlt) : Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightShift);
        bool leftHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        if (rightHeld || leftHeld)
        {
            ChangeGravity();
        }
    }

    private void FixedUpdate()
    {
        if (gravityDirection != Vector3.zero)
        {
            UpdateGravity(gravityDirection);
            gravityDirection = Vector3.zero;
        }
        Movement();
    }

    private void ChangeGravity()
    {
        if (!isGrounded) return;

        // Keymaps for player one and player two to handle gravity directions
        var playerOneKeyMap = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, Vector3.up },
            { KeyCode.S, Vector3.down },
            { KeyCode.D, Vector3.right },
            { KeyCode.A, Vector3.left }
        };

        var playerTwoKeyMap = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.UpArrow, Vector3.up },
            { KeyCode.DownArrow, Vector3.down },
            { KeyCode.RightArrow, Vector3.right },
            { KeyCode.LeftArrow, Vector3.left }
        };

        // Determine which keymap to use based on CompanionMode
        Dictionary<KeyCode, Vector3> currentKeyMap = PlayerManager.instance.CompanionMode ? playerTwoKeyMap : playerOneKeyMap;

        // Check key presses and update grav direction
        foreach (var entry in currentKeyMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                gravityDirection = entry.Value;

                // Rotate the player to match the new gravity direction
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.0f);;
                isGrounded = false;
                break;
            }
        }

    }

    private void UpdateGravity(Vector3 direction)
    {
        Vector3 newGravity = direction * gravity * gravityScale;
        Physics.gravity = newGravity;
    }


    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            isJumping = false;
            jumpCounter = 1;
        }
    }

    private void Movement()
    {
        Vector3 movementDirection = Vector3.zero;
        float effectiveSpeed = speed * (1 - groundDrag) * Time.deltaTime;

        // Dictionary to map input keys to movement directions
        var directionMappings = new Dictionary<KeyCode, Vector3>
        {
            { up, Vector3.up },
            { down, Vector3.down },
            { left, Vector3.left },
            { right, Vector3.right }
        };

        foreach (var mapping in directionMappings)
        {
            if (Input.GetKey(mapping.Key))
            {
                // Check if the player is trying to jump
                if (Physics.gravity.normalized == -mapping.Value && isGrounded)
                {
                    PerformJump();
                }
                else
                {
                    // Add the movement direction to the player's current position
                    movementDirection += mapping.Value * effectiveSpeed;
                }
            }
        }
        // Handle player movement
        rb.MovePosition(transform.position + movementDirection);
    }

    public void PerformJump()
    {
        if (!isJumping && jumpCounter > 0) // Check to make sure the player isn't already jumping
        {
            Vector3 jumpDirection = -Physics.gravity.normalized;
            rb.AddForce(jumpDirection * jumpMultiplier, ForceMode.Impulse);
            isJumping = true;
        }
    }
}