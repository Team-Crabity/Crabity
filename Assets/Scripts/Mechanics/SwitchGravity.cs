using System.Collections;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    [Header("Player Selection")]
    public bool playerOne;
    public bool playerTwo;

    [Header("Gravity")]
    public float gravityScale = 5.0f;
    public float gravity = 9.81f;

    [Header("Movement")]
    [Range(5f, 10f)] public float speed = 7.0f;
    [Range(0f, 0.5f)] public float groundDrag = 0.5f;
    // [Range(0f, 0.5f)] public float airDrag = 0.25f; //Can add different drag cofeffcient when the player is in the air for faster speed

    [Header("Jumping")]
    public int jumpCounter = 1;
    public float jumpMultiplier;
    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }

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

        UpdateGravity(Vector3.down);
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
        bool altHeld = Input.GetKey(KeyCode.RightAlt);
        bool cHeld = Input.GetKey(KeyCode.C);
        
        if (altHeld || cHeld)
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
        bool mode = PlayerManager.instance.CompanionMode;
        if (mode && isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gravityDirection = Vector3.up;
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gravityDirection = Vector3.down;
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gravityDirection = Vector3.right;
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gravityDirection = Vector3.left;
                isGrounded = false;
            }
        }
        else if (!mode && isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                gravityDirection = Vector3.up;
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                gravityDirection = Vector3.down;
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                gravityDirection = Vector3.right;
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                gravityDirection = Vector3.left;
                isGrounded = false;
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
        if (collision.gameObject.tag == "Brick" || collision.gameObject.tag == "Wood" ||
            collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Ground")
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
        if (Input.GetKey(up))
        {
            if (Physics.gravity.normalized == Vector3.down && isGrounded)
            {
                // Debug.Log("Current gravity " + Physics.gravity.normalized + ". Jumping using " + up + " key.");
                PerformJump();
            }
            else
            {
                // Debug.Log("Regular upwards movement.");
                movementDirection += Vector3.up * effectiveSpeed;
            }
        }
        if (Input.GetKey(down))
        {
            if (Physics.gravity.normalized == Vector3.up && isGrounded)
            {
                // Debug.Log("Current gravity " + Physics.gravity.normalized + ". Jumping using " + down + " key.");
                PerformJump();
            }
            else
            {
                // Debug.Log("Regular downwards movement.");
                movementDirection += Vector3.down * effectiveSpeed;
            }
        }
        if (Input.GetKey(left))
        {
            if (Physics.gravity.normalized == Vector3.right && isGrounded)
            {
                // Debug.Log("Current gravity " + Physics.gravity.normalized + ". Jumping using " + left + " key.");
                PerformJump();
            }
            else
            {
                // Debug.Log("Regular left movement.");
                movementDirection += Vector3.left * effectiveSpeed;
            }
        }
        if (Input.GetKey(right))
        {
            if (Physics.gravity.normalized == Vector3.left && isGrounded)
            {
                // Debug.Log("Current gravity " + Physics.gravity.normalized + ". Jumping using " + right + " key.");
                PerformJump();
            }
            else
            {
                // Debug.Log("Regular right movement.");
                movementDirection += Vector3.right * effectiveSpeed;
            }
        }
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
