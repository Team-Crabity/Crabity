using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // GameObject to rotate about
    // public GameObject otherObject;

    // Variables for rotationg
    public float turnTime = 1f;
    private bool rotating = false;

    // Update is called once per frame
    void Update()
    {
        if(!isRotating() && Input.GetKey(KeyCode.LeftShift))
        {
            RotationInput(KeyCode.D, Vector3.up);
            RotationInput(KeyCode.A, Vector3.up * -1);
            RotationInput(KeyCode.S, Vector3.right);
            RotationInput(KeyCode.W, Vector3.right * -1);
            RotationInput(KeyCode.E, Vector3.forward);
            RotationInput(KeyCode.Q, Vector3.forward * -1);
        }
    }

    void RotationInput(KeyCode keyCode, Vector3 axis)
    {
        Vector3 centerPoint = GetCenterPoint();
        
        if (Input.GetKeyDown(keyCode))
        {
            StartCoroutine(Rotate(transform, centerPoint, axis, 90, turnTime));
        }
    }

    public bool isRotating()
    {
        return rotating;
    }

    public Vector3 GetCenterPoint()
    {
        Transform playerOneTransform = global::PlayerManager.instance.playerOne.transform;
        Transform playerTwoTransform = global::PlayerManager.instance.playerTwo.transform;

        if (playerOneTransform == null && playerTwoTransform == null)
        {
            return transform.position;
        }
        else if (playerTwoTransform == null)  // TODO: have it also check if splitscreen camera is active
        {
            return playerOneTransform.position;
        }
        else
        {
            return (playerOneTransform.position + playerTwoTransform.position) / 2;
        }
    }

    // https://forum.unity.com/threads/rotating-exactly-90-degrees-specific-direction-answered.44056/
    IEnumerator Rotate(Transform thisTransform, Vector3 rotateAround, Vector3 rotateAxis, float degrees, float totalTime)
    {
        Transform playerOneTransform = global::PlayerManager.instance.playerOne.transform;
        Transform playerTwoTransform = global::PlayerManager.instance.playerTwo.transform;

        rotating = true;

        var startRotation = thisTransform.rotation;
        var startPosition = thisTransform.position;
        transform.RotateAround(rotateAround, rotateAxis, degrees);
        var endRotation = thisTransform.rotation;
        var endPosition = thisTransform.position;
        thisTransform.rotation = startRotation;
        thisTransform.position = startPosition;

        var rate = degrees / totalTime;

        for (float i = 0; i < degrees; i += Time.deltaTime * rate)
        {
            thisTransform.RotateAround(rotateAround, rotateAxis, Time.deltaTime * rate);
            playerOneTransform.RotateAround(playerOneTransform.position, rotateAxis, -Time.deltaTime * rate);
            playerTwoTransform.RotateAround(playerTwoTransform.position, rotateAxis, -Time.deltaTime * rate);
            yield return null;
        }

        thisTransform.rotation = endRotation;
        thisTransform.position = endPosition;
        rotating = false;
    }
}