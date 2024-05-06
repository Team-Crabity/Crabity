using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Range(5f, 20f)] public float speed = 15.0f;
    [Range(0f, 1f)] public float groundDrag = 0.5f;
    [Range(0f, 0.5f)] public float airDrag = 0.25f; //Can add different drag cofeffcient when the player is in the air for faster speed

    [Header("Jumping")]
    public int jumpCounter = 1;
    public float jumpMultiplier = 10.0f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    public bool isJumping;
    public bool isGrounded;

    private Rigidbody rb;
    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;

    private void Awake()
    {
        float scale = gameObject.GetComponent<SwitchGravity>().gravityScale;
        Physics.gravity = new Vector3(0, -9.81f * scale, 0);
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Set keybinds based on player selection
        if (PlayerManager.instance.IsPlayerOne(gameObject))
        {
            up = KeyCode.W;
            left = KeyCode.A;
            down = KeyCode.S;
            right = KeyCode.D;
        }
        else if (PlayerManager.instance.IsPlayerTwo(gameObject))
        {
            up = KeyCode.UpArrow;
            left = KeyCode.LeftArrow;
            down = KeyCode.DownArrow;
            right = KeyCode.RightArrow;
        }
    }

    private void MovePlayer()
    {
        Vector3 movementDirection = Vector3.zero;
        float effectiveSpeed = speed * (1 - groundDrag) * Time.deltaTime;
        if(isJumping)
        {
            effectiveSpeed = speed * (1 - airDrag) * Time.deltaTime;
        }

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
                if (Physics.gravity.normalized == -mapping.Value && !isJumping) // Check if the player is trying to jump
                {
                    PerformJump(mapping.Key);
                }
                else
                {
                    // Add the movement direction to the player's current position
                    movementDirection += mapping.Value * effectiveSpeed;
                }
            }
        // Handle player movement
        rb.MovePosition(transform.position + movementDirection);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void PerformJump(KeyCode jumpButton)
    {
        isJumping = true;
        Vector3 jumpDirection = -Physics.gravity.normalized;
        rb.velocity += jumpDirection * jumpMultiplier;
        StartCoroutine(JumpRoutine(jumpButton, jumpDirection));
    }

    private IEnumerator JumpRoutine(KeyCode jumpButton, Vector3 jumpDirection)
    {
        bool buttonHeld = true;
        while(buttonHeld)
        {
            if(!Input.GetKeyDown(jumpButton))
            {
                buttonHeld = false;
            }

            if(Vector3.Dot(rb.velocity, jumpDirection) > 0 && !buttonHeld) // Apply low jump multiplier if the jump button is released
            {
                Debug.Log("Low jump multiplier applied, player is moving in the " + Vector3.Dot(rb.velocity, jumpDirection) + " direction. ");
                rb.velocity += jumpDirection * Physics.gravity.magnitude * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            yield return null;
        }
        while (Vector3.Dot(rb.velocity, jumpDirection) < 0 && !buttonHeld) // Fall faster if the jump button is released
        {
            Debug.Log("Fall multiplier applied, player is moving in the " + Vector3.Dot(rb.velocity, jumpDirection) + " direction. ");
            rb.velocity += jumpDirection * Physics.gravity.magnitude * (fallMultiplier - 1) * Time.deltaTime;
            yield return null;
        }
    }

    /*TODO: Update the way isGrounded is determined
    Currently allows for infinite jumps to occur when jumping on walls.
    Jumping along the corner of an object allows the player to gain unintended velocity.
    */
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
}
