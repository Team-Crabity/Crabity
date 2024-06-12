using System.Collections.Generic;
using UnityEngine;

public class NewMove : MonoBehaviour
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
    public bool isJumping;
    public bool isGrounded;
    public float jumpForce = 10f; // Use Unity's default gravity value

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

    private KeyCode upPlayerTwo;
    private KeyCode downPlayerTwo;
    private KeyCode leftPlayerTwo;
    private KeyCode rightPlayerTwo;
    private KeyCode jumpKeyPlayerTwo;

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
            jumpKey = KeyCode.W;
        }
        else if (PlayerManager.instance.IsPlayerTwo(gameObject))
        {
            upPlayerTwo = KeyCode.UpArrow;
            leftPlayerTwo = KeyCode.LeftArrow;
            downPlayerTwo = KeyCode.DownArrow;
            rightPlayerTwo = KeyCode.RightArrow;
            jumpKeyPlayerTwo = KeyCode.UpArrow;
        }
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

        if (rightHeld || leftHeld && isGrounded)
        {
            ChangeGravity();
        }
        else if (Input.GetKeyDown(jumpKey) || Input.GetKeyDown(jumpKeyPlayerTwo))
        {
            PerformJump();
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

                if (entry.Value == Vector3.up)
                {
                    upGravity = true;
                    downGravity = false;
                    leftGravity = false;
                    rightGravity = false;
                }
                else if (entry.Value == Vector3.down)
                {
                    downGravity = true;
                    upGravity = false;
                    leftGravity = false;
                    rightGravity = false;
                }
                else if (entry.Value == Vector3.left)
                {
                    leftGravity = true;
                    rightGravity = false;
                    upGravity = false;
                    downGravity = false;
                }
                else if (entry.Value == Vector3.right)
                {
                    rightGravity = true;
                    leftGravity = false;
                    upGravity = false;
                    downGravity = false;
                }

                gravityDirection = entry.Value;
                RotatePlayer(PlayerManager.instance.playerOne);
                RotatePlayer(PlayerManager.instance.playerTwo);
                isGrounded = false;
                gravitySwitchCount += 1;
                GetComponentInChildren<GravitySounds>().PlayGravitySound();
                UpdateGravity(gravityDirection);
                UpdateKeyMappings(gravityDirection);
                break;
            }
        }
    }

    private void UpdateKeyMappings(Vector3 newGravityDirection)
    {
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

            upPlayerTwo = KeyCode.UpArrow;
            leftPlayerTwo = KeyCode.LeftArrow;
            rightPlayerTwo = KeyCode.RightArrow;
            jumpKeyPlayerTwo = KeyCode.DownArrow;
            playerTwoKeyMap.Add(upPlayerTwo, Vector3.up);
            playerTwoKeyMap.Add(leftPlayerTwo, -Vector3.right);
            playerTwoKeyMap.Add(rightPlayerTwo, Vector3.right);
            playerTwoKeyMap.Add(jumpKeyPlayerTwo, -Vector3.up);
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

            downPlayerTwo = KeyCode.DownArrow;
            leftPlayerTwo = KeyCode.LeftArrow;
            rightPlayerTwo = KeyCode.RightArrow;
            jumpKeyPlayerTwo = KeyCode.UpArrow;
            playerTwoKeyMap.Add(jumpKeyPlayerTwo, Vector3.up);
            playerTwoKeyMap.Add(downPlayerTwo, -Vector3.up);
            playerTwoKeyMap.Add(leftPlayerTwo, -Vector3.right);
            playerTwoKeyMap.Add(rightPlayerTwo, Vector3.right);
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

            upPlayerTwo = KeyCode.UpArrow;
            downPlayerTwo = KeyCode.DownArrow;
            rightPlayerTwo = KeyCode.RightArrow;
            jumpKeyPlayerTwo = KeyCode.LeftArrow;
            playerTwoKeyMap.Add(upPlayerTwo, Vector3.up);
            playerTwoKeyMap.Add(downPlayerTwo, -Vector3.up);
            playerTwoKeyMap.Add(jumpKeyPlayerTwo, -Vector3.right);
            playerTwoKeyMap.Add(rightPlayerTwo, Vector3.right);
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

            upPlayerTwo = KeyCode.UpArrow;
            downPlayerTwo = KeyCode.DownArrow;
            leftPlayerTwo = KeyCode.LeftArrow;
            jumpKeyPlayerTwo = KeyCode.RightArrow;
            playerTwoKeyMap.Add(upPlayerTwo, Vector3.up);
            playerTwoKeyMap.Add(downPlayerTwo, -Vector3.up);
            playerTwoKeyMap.Add(leftPlayerTwo, -Vector3.right);
            playerTwoKeyMap.Add(jumpKeyPlayerTwo, Vector3.right);
        }

    }


    private void UpdateGravity(Vector3 direction)
    {
        Vector3 newGravity = Vector3.zero;

        if (direction == Vector3.up)
        {
            newGravity = new Vector3(0, 9.81f, 0);
        }
        else if (direction == Vector3.down)
        {
            newGravity = new Vector3(0, -9.81f, 0);
        }
        else if (direction == Vector3.left)
        {
            newGravity = new Vector3(-9.81f, 0, 0);
        }
        else if (direction == Vector3.right)
        {
            newGravity = new Vector3(9.81f, 0, 0);
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

        if (PlayerManager.instance.IsPlayerOne(gameObject))
        {
            foreach (var mapping in playerOneKeyMap)
            {
                if (Input.GetKey(mapping.Key) && mapping.Key != jumpKey)
                {
                    movementDirection += mapping.Value;
                }
            }
        }
        else if (PlayerManager.instance.IsPlayerTwo(gameObject))
        {
            foreach (var mapping in playerTwoKeyMap)
            {
                if (Input.GetKey(mapping.Key) && mapping.Key != jumpKeyPlayerTwo)
                {
                    movementDirection += mapping.Value;
                }
            }
        }

        movementDirection.Normalize();
        movementDirection *= effectiveSpeed;

        if (upGravity || downGravity)
        {
            rb.velocity = new Vector3(movementDirection.x, rb.velocity.y, movementDirection.z);
        }
        else if (leftGravity || rightGravity)
        {
            rb.velocity = new Vector3(rb.velocity.x, movementDirection.y, movementDirection.z);
        }
    }

    public void PerformJump()
    {
        if (isGrounded)
        {
            if (upGravity)
            {
                rb.velocity += Vector3.down * jumpForce;
            }
            else if (rightGravity)
            {
                rb.velocity += Vector3.left * jumpForce;
            }
            else if (leftGravity)
            {
                rb.velocity += Vector3.right * jumpForce;
            }
            else if (downGravity)
            {
                if (gravitySwitchCount == 0)
                {
                    rb.velocity += Vector3.up * 16f;
                }
                else
                {
                    rb.velocity += Vector3.up * jumpForce;
                }
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


