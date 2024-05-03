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
    public float jumpMultiplier = 15.0f;
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
        if(!isGrounded)
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
    
    private void FixedUpdate()
    {
        MovePlayer();
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

    /*TODO: Update the way isGrounded is determined
    Currently allows for funky superjumps to occur when jumping along walls
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
