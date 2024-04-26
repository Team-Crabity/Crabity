using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    //IN-PROGRESS: Change camera to adaptive split screen instead of regular camera
    //CREDITS: Brackeys - https://www.youtube.com/watch?v=aLpixrPvlB8
    [Header("Players to Track")]
    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;

    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject playerOneCamera;
    public GameObject playerTwoCamera;

    [Header("Door")]
    public GameObject doorCamera;
    private bool focusingOnDoor = false;
    private bool finishedCutscene = false;

    private Vector3 velocity;
    public bool isSplitScreen = false;

    void LateUpdate()
    {
        Transform playerOneTransform = PlayerManager.instance.playerOne.transform;
        Transform playerTwoTransform = PlayerManager.instance.playerTwo.transform;

        if (targets.Count == 0) { return; }

        if (PlayerManager.instance.CompanionMode && !targets.Contains(playerTwoTransform))
        {
            targets.Add(playerTwoTransform);
        }
        else if (!PlayerManager.instance.CompanionMode && targets.Contains(playerTwoTransform))
        {
            targets.Remove(playerTwoTransform);
        }

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, newPosition, ref velocity, smoothTime);

        if(PressurePlateManager.instance.allPlatesPressed && !finishedCutscene)
        {
            FocusOnDoor();
        }

        if (PlayerManager.instance.CompanionMode)
        {
            if ((OutOfBounds(playerOneTransform) && !focusingOnDoor) ||
                (OutOfBounds(playerTwoTransform) && !focusingOnDoor))
            {
                playerOneCamera.SetActive(true);
                playerTwoCamera.SetActive(true);
                isSplitScreen = true;
            }
            else
            {
                DisableSplitScreen();
            }
        }
        else
        {
            DisableSplitScreen();
        }
    }

    void DisableSplitScreen()
    {
        playerOneCamera.SetActive(false);
        playerTwoCamera.SetActive(false);
        isSplitScreen = false;
    }

    private bool OutOfBounds(Transform target)
    {
        Vector3 screenPoint = mainCamera.GetComponent<Camera>().WorldToViewportPoint(target.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    public Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    private void FocusOnDoor()
    {
        StartCoroutine(FocusSequence());
    }

    private IEnumerator FocusSequence()
    {
        focusingOnDoor = true;
        doorCamera.SetActive(true);
        GetComponent<AudioSource>().Play();
        
        yield return new WaitForSeconds(3.0f);

        finishedCutscene = true;
        focusingOnDoor = false;
        doorCamera.SetActive(false);
        Debug.Log("Door camera sequence complete");
    }
}
