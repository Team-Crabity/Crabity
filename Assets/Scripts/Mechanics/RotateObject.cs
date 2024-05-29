using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    [Header ("Audio")]
    [SerializeField] private List<AudioClip> RotationSounds;
    private AudioSource Source;


    [Header("Rotation")]
    public float turnTime = 0.33f;


    [Header("Reverse Rotation")]
    public bool reverseRotation = false;


    [Header("PlayerOne and PlayerTwo Cameras")]
    public Camera playerOneCamera;
    public Camera playerTwoCamera;

    private bool rotating = false;
    private Transform playerOneTransform;
    private Transform playerTwoTransform;
    private Dictionary<KeyCode, Vector3> keyRotationMap;

    // Track number of rotations
    public static int numRotations;

    void Start()
    {
        Source = GetComponent<AudioSource>();

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
                    RotationInput(entry.Value);
                }
            }
        }
    }

    void RotationInput(Vector3 axis)
    {
        Vector3 centerPoint = GetCenterPoint();

        if (!isRotating())
        {
            if(!reverseRotation)
            {
                StartCoroutine(Rotate(transform, centerPoint, axis, 90, turnTime));
                numRotations += 1;
            }
            else
            {
                StartCoroutine(Rotate(transform, centerPoint, axis * -1, 90, turnTime));
                numRotations += 1;
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
        AudioClip clip = null;
        clip = RotationSounds[Random.Range(0,RotationSounds.Count)];
        Source.clip = clip;
        Source.volume = (1f);
        Source.Play();

        rotating = true;

        // Set smooth time for playerOneCamera and playerTwoCamera to 0 to prevent camera from moving during rotation
        float originalSmoothTime = playerOneCamera.GetComponent<PlayerFollow>().smoothTime;
        playerOneCamera.GetComponent<PlayerFollow>().smoothTime = 0f;
        playerTwoCamera.GetComponent<PlayerFollow>().smoothTime = 0f;

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

        playerOneCamera.GetComponent<PlayerFollow>().smoothTime = originalSmoothTime;
        playerTwoCamera.GetComponent<PlayerFollow>().smoothTime = originalSmoothTime;

        thisTransform.rotation = endRotation;
        thisTransform.position = endPosition;

        rotating = false;
    }
}