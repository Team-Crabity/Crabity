using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpForce = 3.5f;
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
            Vector3 jumpDirection = -Physics.gravity;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpsLeft--;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpsLeft = maxJumps;
        }
    }

}
