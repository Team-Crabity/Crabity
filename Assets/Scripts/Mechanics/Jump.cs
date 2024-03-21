using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jumps")]
    public float jumpForce = 12.5f;
    public int jumpsLeft = 2;
    public int maxJumps = 2;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (jumpsLeft > 0))
        {
            PerformJump();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Brick" || collision.gameObject.tag == "Wood" ||
            collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Ground")
        {
            jumpsLeft = maxJumps;
        }
    }

    private void PerformJump()
    {
        {
            Vector3 jumpDirection = -Physics.gravity.normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.VelocityChange);
            jumpsLeft--;
        }
    }
}
