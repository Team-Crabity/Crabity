using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Range(5f, 20f)] public float speed = 15.0f;
    [Range(0f, 1f)] public float groundDrag = 0.5f;
    [Range(0f, 0.5f)] public float airDrag = 0.25f;

    [Header("Gravity")]
    public bool gravityOnCooldown = false;
    public bool localGravityZone;
    public int gravitySwitchCount = 0;

    [Header("RotatingObjects Parent")]
    public GameObject RotatingObjects;

    [Header("Jumping")]
    public int jumpCounter = 1;
    public float jumpMultiplier = 1.0f;
    public bool isJumping;
    public bool isGrounded;

    [Header("Colliders")]
    public Collider collider;
    public float skinWidth = 0.02f;

    [Header("Audio")]
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private List<AudioClip> WalkingSounds;
    private AudioSource Source;

    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.down;
    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;
    private KeyCode jumpKey;

    private bool upGravity = false;
    private bool downGravity = true;
    private bool leftGravity = false;
    private bool rightGravity = false;

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
        Physics.gravity = new Vector3(0, -9.81f, 0); // Default gravity

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

        jumpKey = up; // Set initial jump key
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleInput();

        if (!isGrounded && !isJumping)
        {
            rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        RoundRotationToNearest90Degrees();
    }

    private void RoundRotationToNearest90Degrees()
    {
        Vector3 euler = transform.rotation.eulerAngles;

        euler.x = RoundToNearest90(euler.x);
        euler.y = RoundToNearest90(euler.y);
        euler.z = RoundToNearest90(euler.z);

        transform.rotation = Quaternion.Euler(euler);
    }

    private float RoundToNearest90(float angle)
    {
        return Mathf.Round(angle / 90.0f) * 90.0f;
    }

    public void HandleInput()
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
        else
        {
            MovePlayer();
        }
    }

    public void ChangeGravity()
    {
        if (gravityOnCooldown || localGravityZone) return;

        Dictionary<KeyCode, Vector3> currentKeyMap = PlayerManager.instance.CompanionMode ? playerTwoKeyMap : playerOneKeyMap;

        foreach (var entry in currentKeyMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                gravityOnCooldown = true;

                // Handle gravity direction flags
                if (entry.Value == Vector3.up)
                {
                    upGravity = true;
                    downGravity = false;
                    leftGravity = false;
                    rightGravity = false;
                    Debug.Log("Gravity changed to Up");
                }
                else if (entry.Value == Vector3.down)
                {
                    downGravity = true;
                    upGravity = false;
                    leftGravity = false;
                    rightGravity = false;
                    Debug.Log("Gravity changed to Down");
                }
                else if (entry.Value == Vector3.left)
                {
                    leftGravity = true;
                    rightGravity = false;
                    upGravity = false;
                    downGravity = false;
                    Debug.Log("Gravity changed to Left");
                }
                else if (entry.Value == Vector3.right)
                {
                    rightGravity = true;
                    leftGravity = false;
                    upGravity = false;
                    downGravity = false;
                    Debug.Log("Gravity changed to Right");
                }

                gravityDirection = entry.Value;
                RotatePlayer(PlayerManager.instance.playerOne);
                RotatePlayer(PlayerManager.instance.playerTwo);
                isGrounded = false;
                gravitySwitchCount += 1;
                GetComponentInChildren<GravitySounds>().PlayGravitySound();
                UpdateKeyMappings(gravityDirection);
                Debug.Log($"Gravity direction: {gravityDirection}");
                break;
            }
        }
    }

    private void UpdateKeyMappings(Vector3 newGravityDirection)
    {
        // Clear the existing key mappings
        playerOneKeyMap.Clear();
        playerTwoKeyMap.Clear();

        if (newGravityDirection == Vector3.up)
        {
            up = KeyCode.W;
            left = KeyCode.A;
            right = KeyCode.D;
            jumpKey = KeyCode.S;
            playerOneKeyMap.Add(up, Vector3.up);
            playerOneKeyMap.Add(jumpKey, -Vector3.up);
            playerOneKeyMap.Add(left, -Vector3.right);
            playerOneKeyMap.Add(right, Vector3.right);
            Debug.Log($"Gravity direction: Up {Vector3.up}");
        }
        else if (newGravityDirection == Vector3.down)
        {
            down = KeyCode.S;
            left = KeyCode.A;
            right = KeyCode.D;
            jumpKey = KeyCode.W;
            playerOneKeyMap.Add(jumpKey, Vector3.up);
            playerOneKeyMap.Add(down, -Vector3.up);
            playerOneKeyMap.Add(left, -Vector3.right);
            playerOneKeyMap.Add(right, Vector3.right);
            Debug.Log($"Gravity direction: Down {Vector3.down}");
        }
        else if (newGravityDirection == Vector3.right)
        {
            up = KeyCode.W;
            down = KeyCode.S;
            right = KeyCode.D;
            jumpKey = KeyCode.A;
            playerOneKeyMap.Add(up, Vector3.up);
            playerOneKeyMap.Add(down, -Vector3.up);
            playerOneKeyMap.Add(jumpKey, -Vector3.right);
            playerOneKeyMap.Add(right, Vector3.right);
            Debug.Log($"Gravity direction: Right {Vector3.right}");
        }
        else if (newGravityDirection == Vector3.left)
        {
            up = KeyCode.W;
            down = KeyCode.S;
            left = KeyCode.A;
            jumpKey = KeyCode.D;
            playerOneKeyMap.Add(up, Vector3.up);
            playerOneKeyMap.Add(down, -Vector3.up);
            playerOneKeyMap.Add(left, -Vector3.right);
            playerOneKeyMap.Add(jumpKey, Vector3.right);
            Debug.Log($"Gravity direction: Left {Vector3.left}");
        }

        playerTwoKeyMap.Add(up, Vector3.up);
        playerTwoKeyMap.Add(down, -Vector3.up);
        playerTwoKeyMap.Add(left, -Vector3.right);
        playerTwoKeyMap.Add(right, Vector3.right);
    }

    private void UpdateGravity(Vector3 direction)
    {
        // Calculate new gravity vector based on direction
        Vector3 newGravity = Vector3.zero;

        if (direction == Vector3.up)
        {
            newGravity = new Vector3(0, 9.81f, 0); // Gravity along negative Y-axis
        }
        else if (direction == Vector3.down)
        {
            newGravity = new Vector3(0, -9.81f, 0); // Gravity along positive Y-axis
        }
        else if (direction == Vector3.left)
        {
            newGravity = new Vector3(-9.81f, 0, 0); // Gravity along positive X-axis
        }
        else if (direction == Vector3.right)
        {
            newGravity = new Vector3(9.81f, 0, 0); // Gravity along negative X-axis
        }

        Physics.gravity = newGravity;
    }

    private void RotatePlayer(GameObject player)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, -gravityDirection) * player.transform.rotation;
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 1.0f);
    }

    public void MovePlayer()
    {
        Vector3 movementDirection = Vector3.zero;
        float effectiveSpeed = speed * (isGrounded ? (1 - groundDrag) : (1 - airDrag));

        // Check if jump key is pressed
        if (Input.GetKeyDown(jumpKey))
        {
            PerformJump();
        }

        // Adjust movement direction based on the current gravity direction
        foreach (var mapping in playerOneKeyMap)
        {
            // Adjust movement direction based on the current gravity direction
            if (Input.GetKey(mapping.Key) && mapping.Key != jumpKey)
            {
                if (upGravity)
                {
                    // For up gravity, A/D moves horizontally, W/S moves vertically
                    if (mapping.Value == Vector3.left)
                        movementDirection += Vector3.left;
                    else if (mapping.Value == Vector3.right)
                        movementDirection += Vector3.right;
                    else if (mapping.Value == Vector3.up)
                        movementDirection += Vector3.up;
                    else if (mapping.Value == Vector3.down)
                        movementDirection += Vector3.down;
                }
                else if (downGravity)
                {
                    // For down gravity, A/D moves horizontally, W/S moves vertically (inverse)
                    if (mapping.Value == Vector3.left)
                        movementDirection += Vector3.right; // Move right for A
                    else if (mapping.Value == Vector3.right)
                        movementDirection += Vector3.left; // Move left for D
                    else if (mapping.Value == Vector3.up)
                        movementDirection += Vector3.down; // Move down for W
                    else if (mapping.Value == Vector3.down)
                        movementDirection += Vector3.up; // Move up for S
                }
                else if (leftGravity)
                {
                    // For left gravity, W/S moves horizontally, A/D moves vertically
                    if (mapping.Value == Vector3.up)
                        movementDirection += Vector3.left; // Move left for W
                    else if (mapping.Value == Vector3.down)
                        movementDirection += Vector3.right; // Move right for S
                    else if (mapping.Value == Vector3.left)
                        movementDirection += Vector3.down;
                    else if (mapping.Value == Vector3.right)
                        movementDirection += Vector3.up;
                }
                else if (rightGravity)
                {
                    // For right gravity, W/S moves horizontally, A/D moves vertically
                    if (mapping.Value == Vector3.up)
                        movementDirection += Vector3.right; // Move right for W
                    else if (mapping.Value == Vector3.down)
                        movementDirection += Vector3.left; // Move left for S
                    else if (mapping.Value == Vector3.left)
                        movementDirection += Vector3.up;
                    else if (mapping.Value == Vector3.right)
                        movementDirection += Vector3.down;
                }
            }
        }

        // Normalize movement direction and apply speed
        movementDirection.Normalize();
        movementDirection *= effectiveSpeed;

        // Apply movement to the rigidbody
        rb.velocity = new Vector3(movementDirection.x, rb.velocity.y, movementDirection.z);
    }




    public void PerformJump()
    {
        if (isGrounded)
        {
            float jumpForce = Mathf.Sqrt(2 * jumpMultiplier * 9.81f); // Use Unity's default gravity value
            if (upGravity == true)
            {
                Debug.Log($"up gravity!: {Vector3.down}");
                rb.velocity += Vector3.down * jumpForce;
            }
            else if (rightGravity == true)
            {
                Debug.Log($"right gravity!: {Vector3.left}");
                rb.velocity += Vector3.left * jumpForce;
            }
            else if (leftGravity == true)
            {
                Debug.Log($"left gravity!: {Vector3.right}");
                rb.velocity += Vector3.right * jumpForce;
            }
            else // downGravity
            {
                Debug.Log($"down gravity!: {Vector3.up}");
                rb.velocity += Vector3.up * jumpForce;
            }
            Source.PlayOneShot(JumpSound);
        }
        else
        {
            Debug.Log("Cannot jump, not grounded");
        }
    }

    public bool IsGrounded()
    {
        int layerMask = 1 << 10;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + transform.TransformDirection(Vector3.down) * 1f;

        Debug.DrawRay(rayOrigin, transform.TransformDirection(Vector3.down) * 0.3f, Color.red);
        Debug.DrawRay(rayOrigin + new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down) * 0.3f, Color.red);
        Debug.DrawRay(rayOrigin - new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down) * 0.3f, Color.red);

        if (Physics.Raycast(rayOrigin, transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask) ||
            Physics.Raycast(rayOrigin + new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask) ||
            Physics.Raycast(rayOrigin - new Vector3(0.75f, 0, 0), transform.TransformDirection(Vector3.down), out hit, 0.3f, layerMask))
        {
            gravityOnCooldown = false;
            return true;
        }
        return false;
    }
}
