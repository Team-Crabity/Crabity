using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // GameObject to rotate about
    public GameObject otherObject;

    // Variables for rotating
    private float turnTime = 0.75f;
    private bool rotating = false;

    // Update is called once per frame
    void Update()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);

        // Continue rotating object left
        if (!isRotating() && Input.GetKeyDown(KeyCode.A) && shiftHeld)
        {
            StartCoroutine(RotateAroundCameraAxis(90, turnTime, Vector3.up));
        }

        // Continue rotating object right
        if (!isRotating() && Input.GetKeyDown(KeyCode.D) && shiftHeld)
        {
            StartCoroutine(RotateAroundCameraAxis(-90, turnTime, Vector3.up));
        }

        // Continue rotating object up
        if (!isRotating() && Input.GetKeyDown(KeyCode.W) && shiftHeld)
        {
            StartCoroutine(RotateAroundCameraAxis(90, turnTime, Vector3.right));
        }

        // Continue rotating object down
        if (!isRotating() && Input.GetKeyDown(KeyCode.S) && shiftHeld)
        {
            StartCoroutine(RotateAroundCameraAxis(-90, turnTime, Vector3.right));
        }
    }

    bool isRotating()
    {
        return rotating;
    }

    // Modified code from: https://forum.unity.com/threads/rotating-exactly-90-degrees-specific-direction-answered.44056/
    IEnumerator RotateAroundCameraAxis(float degrees, float totalTime, Vector3 localAxis)
    {
        rotating = true;

        Vector3 worldAxis = transform.TransformDirection(localAxis); // Transform the local axis to a world axis based on the camera's orientation
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(degrees, worldAxis) * startRotation;

        float rate = degrees / totalTime;
        float angleRotated = 0;
        while (angleRotated < Mathf.Abs(degrees))
        {
            float angleToRotate = Time.deltaTime * rate;
            transform.RotateAround(otherObject.transform.position, worldAxis, angleToRotate);
            angleRotated += Mathf.Abs(angleToRotate);
            yield return null;
        }

        // Ensure the rotation is exactly as intended despite frame rate variations
        transform.rotation = endRotation;

        rotating = false;
    }
}
