using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // GameObject to rotate about
    public GameObject otherObject;

    // Variables for rotationg
    public float turnTime = 1f;
    private bool rotating = false;

    // Update is called once per frame
    void Update()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);

        // Continue rotating object left
        if (!isRotating() && Input.GetKeyDown(KeyCode.D) && shiftHeld)
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.up, 90, turnTime));
        }

        // Continue rotating object right
        if (!isRotating() && Input.GetKeyDown(KeyCode.A) && shiftHeld)
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.up * -1, 90, turnTime));
        }

        // Continue rotating object up
        if (!isRotating() && Input.GetKeyDown(KeyCode.S) && shiftHeld)
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.right, 90, turnTime));
        }

        // Continue rotating object down
        if (!isRotating() && Input.GetKeyDown(KeyCode.W) && shiftHeld)
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.right * -1, 90, turnTime));
        }

        // Continue rotating object clockwise
        if (!isRotating() && Input.GetKeyDown(KeyCode.Q) && shiftHeld)
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.forward, 90, turnTime));
        }
        
        // Continue rotating object counterclockwise
        if (!isRotating() && Input.GetKeyDown(KeyCode.E) && shiftHeld)
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.forward * -1, 90, turnTime));
        }
    }

    public bool isRotating()
    {
        return rotating;
    }

    // https://forum.unity.com/threads/rotating-exactly-90-degrees-specific-direction-answered.44056/
    IEnumerator Rotate(Transform thisTransform, Transform otherTransform, Vector3 rotateAxis, float degrees, float totalTime)
    {
        rotating = true;

        var startRotation = thisTransform.rotation;
        var startPosition = thisTransform.position;
        transform.RotateAround(otherTransform.position, rotateAxis, degrees);
        var endRotation = thisTransform.rotation;
        var endPosition = thisTransform.position;
        thisTransform.rotation = startRotation;
        thisTransform.position = startPosition;

        var rate = degrees / totalTime;
        for (float i = 0; i < degrees; i += Time.deltaTime * rate)
        {
            thisTransform.RotateAround(otherTransform.position, rotateAxis, Time.deltaTime * rate);
            yield return null;
        }

        thisTransform.rotation = endRotation;
        thisTransform.position = endPosition;
        rotating = false;
    }
}