using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Rotation")]
    public float turnTime = 0.5f;
    [Header("Set to -1 to reverse rotation")]
    public int rotationInteger = 1;
    public bool reverseRotation = false;

    private bool rotating = false;
    private Transform playerOneTransform;
    private Transform playerTwoTransform;
    private Dictionary<KeyCode, Vector3> keyRotationMap;

    void Start()
    {
        keyRotationMap = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, Vector3.right * -1 },
            { KeyCode.A, Vector3.up * -1 },
            { KeyCode.S, Vector3.right },
            { KeyCode.D, Vector3.up },
            { KeyCode.Q, Vector3.forward * -1 },
            { KeyCode.E, Vector3.forward }
        };
        playerOneTransform = PlayerManager.instance.playerOne.transform;
        playerTwoTransform = PlayerManager.instance.playerTwo.transform;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            foreach (KeyValuePair<KeyCode, Vector3> entry in keyRotationMap)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    Debug.Log("Key Pressed: " + entry.Key + " Value: " + entry.Value);
                    RotationInput(entry.Key, entry.Value);
                }
            }
        }
    }

    void RotationInput(KeyCode keyCode, Vector3 axis)
    {
        Vector3 centerPoint = GetCenterPoint();
        
        if (!isRotating())
        {
            StartCoroutine(Rotate(transform, centerPoint, axis * rotationInteger, 90, turnTime));
        }
    }

    public bool isRotating()
    {
        return rotating;
    }

    public Vector3 GetCenterPoint()
    {
        if (playerOneTransform == null && playerTwoTransform == null)
        {
            return transform.position;
        }
        else if (!PlayerManager.instance.CompanionMode)
        {
            return playerOneTransform.position;
        }
        else
        {
            return (playerOneTransform.position + playerTwoTransform.position) / 2;
        }
    }

    IEnumerator Rotate(Transform thisTransform, Vector3 center, Vector3 rotateAxis, float degrees, float totalTime)
    {
        rotating = true;

        var startRotation = thisTransform.rotation;
        var startPosition = thisTransform.position;
        transform.RotateAround(center, rotateAxis, degrees);
        var endRotation = thisTransform.rotation;
        var endPosition = thisTransform.position;
        thisTransform.rotation = startRotation;
        thisTransform.position = startPosition;

        var rate = degrees / totalTime;

        for (float i = 0; i < degrees; i += Time.deltaTime * rate)
        {
            thisTransform.RotateAround(center, rotateAxis, Time.deltaTime * rate);
            if(!reverseRotation)
            {
                playerOneTransform.RotateAround(playerOneTransform.position, rotateAxis, -Time.deltaTime * rate);
                playerTwoTransform.RotateAround(playerTwoTransform.position, rotateAxis, -Time.deltaTime * rate);
            }
            yield return null;
        }

        thisTransform.rotation = endRotation;
        thisTransform.position = endPosition;

        rotating = false;
    }
}