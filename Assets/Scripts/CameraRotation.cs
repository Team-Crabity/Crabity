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

        // Continue rotating object left
        if (!isRotating() && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.up, 90, turnTime));
        }

        // Continue rotating object right
        if (!isRotating() && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Rotate(transform, otherObject.transform, Vector3.up * -1, 90, turnTime));
        }
    }

    bool isRotating()
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
