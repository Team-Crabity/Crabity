using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Rotation")]
    public float turnTime = 0.5f;

    [Header("Reverse Rotation")]
    public bool reverseRotation = false;

    [Header("PlayerOne and PlayerTwo Cameras")]
    public Camera playerOneCamera;
    public Camera playerTwoCamera;

    private bool rotating = false;
    private Transform playerOneTransform;
    private Transform playerTwoTransform;
    private Dictionary<KeyCode, Vector3> playerOneKeyMap;
    private Dictionary<KeyCode, Vector3> playerTwoKeyMap;

    void Start()
    {
        playerOneTransform = PlayerManager.instance.playerOne.transform;
        playerTwoTransform = PlayerManager.instance.playerTwo.transform;

        playerOneKeyMap = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, Vector3.right * -1 },
            { KeyCode.A, Vector3.up * -1 },
            { KeyCode.S, Vector3.right },
            { KeyCode.D, Vector3.up },
            { KeyCode.Q, Vector3.forward * -1 },
            { KeyCode.E, Vector3.forward }
        };

        playerTwoKeyMap = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.O, Vector3.right * -1 },
            { KeyCode.K, Vector3.up * -1 },
            { KeyCode.L, Vector3.right },
            { KeyCode.Semicolon, Vector3.up },
            { KeyCode.I, Vector3.forward * -1 },
            { KeyCode.P, Vector3.forward }
        };
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            foreach (KeyValuePair<KeyCode, Vector3> entry in playerOneKeyMap)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    RotationInput(entry.Key, entry.Value);
                }
            }
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            foreach (KeyValuePair<KeyCode, Vector3> entry in playerTwoKeyMap)
            {
                if (Input.GetKeyDown(entry.Key))
                {
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
            if(!reverseRotation)
            {
                StartCoroutine(Rotate(transform, centerPoint, axis, 90, turnTime));
            }
            else
            {
                StartCoroutine(Rotate(transform, centerPoint, axis * -1, 90, turnTime));
            }
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

        // Set smooth time for playerOneCamera and playerTwoCamera to 0 to prevent camera from moving during rotation
        float originalSmoothTime = playerOneCamera.GetComponent<PlayerFollow>().smoothTime;

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
            if (!reverseRotation)
            {
                // Rotate the playerOne and playerTwo cameras around the center point in the opposite direction
                playerOneTransform.RotateAround(playerOneTransform.position, rotateAxis, -Time.deltaTime * rate);
                playerTwoTransform.RotateAround(playerTwoTransform.position, rotateAxis, -Time.deltaTime * rate);

                // Set the playerOne and playerTwo cameras smooth time to 0 while rotating
                playerOneCamera.GetComponent<PlayerFollow>().smoothTime = 0f;
                playerTwoCamera.GetComponent<PlayerFollow>().smoothTime = 0f;
            }
            yield return null;
        }
        // Set the playerOne and playerTwo cameras back to their original smooth time
        playerOneCamera.GetComponent<PlayerFollow>().smoothTime = originalSmoothTime;
        playerTwoCamera.GetComponent<PlayerFollow>().smoothTime = originalSmoothTime;

        thisTransform.rotation = endRotation;
        thisTransform.position = endPosition;

        rotating = false;
    }
}