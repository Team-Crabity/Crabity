using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCutscenes : MonoBehaviour
{
    [Header("Rotation")]
    public float turnTime = 0.33f;

    [SerializeField]
    private Animator doorAnimator;

    public Vector3 lastAxis;

    private bool rotating = false;
    private Transform playerOneTransform;
    private Transform playerTwoTransform;
    private Vector3 newGravity;

    void Start()
    {
        playerOneTransform = PlayerManager.instance.playerOne.transform;
        playerTwoTransform = PlayerManager.instance.playerTwo.transform;
    }

    public void RotatePlay()
    {
        // Simulate LShift + W input
        lastAxis = Vector3.right * -1;
        StartCoroutine(Rotate(transform, GetCenterPoint(), lastAxis, 90, turnTime));

        // Reverse gravity
        newGravity = new Vector3(0, 9.81f * 5.0f, 0);
        Physics.gravity = newGravity;
        RotatePlayer(playerOneTransform.gameObject, newGravity);
        RotatePlayer(playerTwoTransform.gameObject, newGravity);

        // Open door and play audio
        doorAnimator.SetBool("character_nearby", true);
        GetComponent<AudioSource>().Play();
    }

    public void RotateSettings()
    {
        // Simulate LShift + A input
        lastAxis = Vector3.up * -1;
        StartCoroutine(Rotate(transform, GetCenterPoint(), lastAxis, 90, turnTime));

        // Set gravity to the right
        newGravity = new Vector3(9.81f * 5.0f, 0, 0);
        Physics.gravity = newGravity;
        RotatePlayer(playerOneTransform.gameObject, newGravity);
        RotatePlayer(playerTwoTransform.gameObject, newGravity);

        // Open settings menu
    }

    public void RotateCoop()
    {
        // Simulate LShift + D input
        lastAxis = Vector3.up;
        StartCoroutine(Rotate(transform, GetCenterPoint(), lastAxis, 90, turnTime));

        // Set gravity to the left
        newGravity = new Vector3(-9.81f * 5.0f, 0, 0);
        Physics.gravity = newGravity;
        RotatePlayer(playerOneTransform.gameObject, newGravity);
        RotatePlayer(playerTwoTransform.gameObject, newGravity);
    }

    public void RotateQuit()
    {
        // Simulate LShift + S input
        lastAxis = Vector3.right;
        StartCoroutine(Rotate(transform, GetCenterPoint(), lastAxis, 90, turnTime));

        // Open door and play audio
        doorAnimator.SetBool("character_nearby", true);
        GetComponent<AudioSource>().Play();
    }

    public void UndoRotation()
    {
        // Undo last rotation
        StartCoroutine(Rotate(transform, GetCenterPoint(), lastAxis * -1, 90, turnTime));

        // Set gravity to normal
        newGravity = new Vector3(0, -9.81f * 5.0f, 0);
        Physics.gravity = newGravity;
        RotatePlayer(playerOneTransform.gameObject, newGravity);
        RotatePlayer(playerTwoTransform.gameObject, newGravity);
    }

    public bool isRotating()
    {
        return rotating;
    }

    private void RotatePlayer(GameObject player, Vector3 gravityDirection)
    {
        // Rotate the player to match the new gravity direction
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, -gravityDirection) * player.transform.rotation;
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 1.0f);
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
        if (isRotating()) yield break;
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
            playerOneTransform.RotateAround(playerOneTransform.position, rotateAxis, -Time.deltaTime * rate);
            playerTwoTransform.RotateAround(playerTwoTransform.position, rotateAxis, -Time.deltaTime * rate);
            yield return null;
        }

        thisTransform.rotation = endRotation;
        thisTransform.position = endPosition;

        rotating = false;
    }
}