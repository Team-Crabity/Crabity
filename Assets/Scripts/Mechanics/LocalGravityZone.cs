using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGravityZone : MonoBehaviour
{
    public Vector3 localGravity = Vector3.down;
    
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.GetComponent<CustomGravity>().enabled = true;
            Movement movement = rb.GetComponent<Movement>();
            movement.localGravityZone = true;
            
            // Rotate player to match the gravity direction of area
            Quaternion targetRotation = Quaternion.FromToRotation(other.transform.up, -Vector3.down) * other.transform.rotation;
            other.transform.rotation = Quaternion.Slerp(other.transform.rotation, targetRotation, 1.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reset to global gravity settings
            rb.useGravity = true;
            rb.GetComponent<CustomGravity>().enabled = false;  // Disable the custom gravity script
            Movement movement = rb.GetComponent<Movement>();
            movement.localGravityZone = false;

            //Rotate player to match the global gravity direction
            Quaternion targetRotation = Quaternion.FromToRotation(other.transform.up, -Physics.gravity.normalized) * other.transform.rotation;
            other.transform.rotation = Quaternion.Slerp(other.transform.rotation, targetRotation, 1.0f);
        }
    }
}
