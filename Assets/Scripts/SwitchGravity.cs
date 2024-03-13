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

    private Rigidbody rb;
    public Vector3 gravityDirection = Vector3.zero;
    private bool gravityCooldown;

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

    public void FixedUpdate()
    {
        if (gravityDirection != Vector3.zero)
        {
            UpdateGravity(gravityDirection);
            gravityDirection = Vector3.zero;
        }
    }

    public void ChangeGravityDirection()
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

}
