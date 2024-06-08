using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Range(5f, 20f)] public float speed = 15.0f; // Default speed value
    [Range(0f, 1f)] public float groundDrag = 0.5f; // Ground drag is more than air drag
    [Range(0f, 0.5f)] public float airDrag = 0.25f; // Air drag is less than ground drag

    [Header("Gravity")]
    public float gravityScale = 5.0f; // Default gravity scale value
    public float gravity = 9.81f; // Default gravity value
    public bool gravityOnCooldown = false; // Check if gravity switch is on cooldown
    public bool localGravityZone; // Check if player is in a local gravity zone
    public int gravitySwitchCount = 0; // Keep track of number of gravity switches

    [Header("RotatingObjects Parent")]
    public GameObject RotatingObjects; // This would be used to reset the parent of the player back to the RotatingObjects game object

    [Header("Jumping")]
    public int jumpCounter = 1; // Number of jumps allowed
    public float jumpMultiplier = 10.0f; // Jump force multiplier
    public bool isJumping; // Check if player is jumping
    public bool isGrounded; // Check if player is grounded

    [Header("Colliders")]
    public Collider collider;
    public float skinWidth = 0.02f;

    [Header("Audio")]
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private List<AudioClip> WalkingSounds;
    private AudioSource Source;

    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.zero;
    private Vector3 jumpDirection = Vector3.zero;
    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;


    // Keymaps for players to be able to switch gravity
    private Dictionary<KeyCode, Vector3> playerOneKeyMap = new Dictionary<KeyCode, Vector3>
    {
        { KeyCode.W, Vector3.up },
        { KeyCode.A, Vector3.left },
        { KeyCode.S, Vector3.down },
        { KeyCode.D, Vector3.right }
    };
    private Dictionary<KeyCode, Vector3> playerTwoKeyMap = new Dictionary<KeyCode, Vector3>
    {
        { KeyCode.UpArrow, Vector3.up },
        { KeyCode.LeftArrow, Vector3.left },
        { KeyCode.DownArrow, Vector3.down },
        { KeyCode.RightArrow, Vector3.right }
    };

    void Start()
    {
        Source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -9.81f * gravityScale, 0);

        // textCooldown.gameObject.SetActive(false);
        // imageCooldown.fillAmount = 0.0f;

        // Default keybinds if player is not assigned
        up = KeyCode.W;
        left = KeyCode.A;
        down = KeyCode.S;
        right = KeyCode.D;

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

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleInput();
        // if (!IsOwner) return;
    }

    private void HandleInput()
    {
        bool rightHeld = Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightAlt);
        bool leftHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        if (gravityDirection != Vector3.zero)
        {
            UpdateGravity(gravityDirection);
            gravityDirection = Vector3.zero;
        }

        if (rightHeld || leftHeld)
        {
            ChangeGravity();
        }
        else if (!rightHeld || !leftHeld)
        {
            MovePlayer();
        }
    }

    public void ChangeGravity()
    {
        if (gravityOnCooldown || localGravityZone) return;
        // Determine which keymap to use based on player selection
        Dictionary<KeyCode, Vector3> currentKeyMap = PlayerManager.instance.CompanionMode ? playerTwoKeyMap : playerOneKeyMap;

        // Check key presses and update grav direction
        foreach (var entry in currentKeyMap)
        {
            if (Input.GetKey(entry.Key))
            {
                gravityOnCooldown = true;

                gravityDirection = entry.Value;


                RotatePlayer(PlayerManager.instance.playerOne);
                RotatePlayer(PlayerManager.instance.playerTwo);

                isGrounded = false;

                // Cooldown UI
                // cooldown.StartCooldown();
                // textCooldown.gameObject.SetActive(true);

                // Update gravity switch count
                gravitySwitchCount += 1;

                // Access child's GravitySounds script and use the PlayGravitySound method
                // This is done to prevent the gravity sound from being cutoff from other sounds
                GetComponentInChildren<GravitySounds>().PlayGravitySound();
                break;
            }
        }
    }

    private void UpdateGravity(Vector3 direction)
    {
        Vector3 newGravity = direction * gravity * gravityScale;
        Physics.gravity = newGravity;
    }

    private void RotatePlayer(GameObject player)
    {
        // Rotate the player to match the new gravity direction
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, -gravityDirection) * player.transform.rotation;
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 1.0f);
    }

    public void MovePlayer()
    {
        Vector3 movementDirection = Vector3.zero;

        // More drag is applied when the player is in the air, less drag is applied when jumping or falling
        float effectiveSpeed = speed * (isGrounded ? (1 - groundDrag) : (1 - airDrag)) * Time.deltaTime;

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
                if (Physics.gravity.normalized == -mapping.Value && !localGravityZone)
                {
                    PerformJump();
                }
                else if (Physics.gravity.normalized != -mapping.Value && !localGravityZone)
                {
                    // Add the movement direction to the player's current position
                    movementDirection += mapping.Value * effectiveSpeed;
                    if (isGrounded && !isJumping)
                    {
                        AudioClip clip = null;
                        clip = WalkingSounds[Random.Range(0, WalkingSounds.Count)];
                        Source.clip = clip;
                        Source.volume = Random.Range(0.01f, 0.02f);
                        Source.pitch = Random.Range(0.05f, 0.15f);
                        Source.Play();
                    }
                }
                else if (localGravityZone && mapping.Value != Vector3.up)
                {
                    movementDirection += mapping.Value * effectiveSpeed;
                }
                else if (localGravityZone && mapping.Value == Vector3.up)
                {
                    PerformJump();
                }
            }
        }

        // Handle player movement
        rb.MovePosition(transform.position + movementDirection);
    }

    public void PerformJump()
    {
        if (!isJumping && jumpCounter == 1) // Check to make sure the player isn't already jumping
        {
            jumpCounter = 0;
            isJumping = true;
            Vector3 jumpDirection = -Physics.gravity.normalized;
            if (localGravityZone)
            {
                jumpDirection = Vector3.up;
            }
            // rb.AddForce(jumpDirection * jumpMultiplier, ForceMode.Impulse);
            rb.velocity = jumpDirection * jumpMultiplier;

            // Stop the walking sound + play the jump sound
            Source.clip = JumpSound;
            Source.volume = (0.5f);
            Source.pitch = Random.Range(0.9f, 1.1f);
            Source.Play();
        }
    }

    public bool IsGrounded()
    {
        int layerMask = 1 << 10;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + transform.TransformDirection(Vector3.down) * 1f;

        // Allow players to stand on top of each other and carry each other around, currently WIP
        // if (Physics.Raycast(rayOrigin, transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask) && hit.collider.CompareTag("Player"))
        // {
        //     // Temporarily attach the player the object until it moves off
        //     transform.parent = hit.collider.transform;
        //     Debug.Log("Player is on top of another player");
        // }
        // else
        // {
        //     transform.parent = RotatingObjects.transform;
        // }

        Debug.DrawRay(rayOrigin, transform.TransformDirection(Vector3.down) * 0.3f, Color.red);
        Debug.DrawRay(rayOrigin + new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down) * 0.3f, Color.red);
        Debug.DrawRay(rayOrigin - new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down) * 0.3f, Color.red);

        if (Physics.Raycast(rayOrigin, transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask) ||
         Physics.Raycast(rayOrigin + new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask) ||
         Physics.Raycast(rayOrigin - new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask))
        {
            gravityOnCooldown = false;
            isJumping = false;
            jumpCounter = 1;
            return true;
        }
        return false;
    }

    // Adjust the collider bounds to prevent the player from falling through the floor by adding a skin width
    void AdjustColliderBounds()
    {
        Bounds bounds;
        bounds = collider.bounds;
        bounds.Expand(-2 * skinWidth);
    }
}
