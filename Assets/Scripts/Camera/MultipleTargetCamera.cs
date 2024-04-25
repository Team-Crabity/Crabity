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

    [Header("Player Game Objects")]
    public GameObject playerOne;
    public GameObject playerTwo;

    [Header("Player Cameras")]
    public GameObject mainCamera;
    public GameObject playerOneCamera;
    public GameObject playerTwoCamera;

    [Header("Door")]
    public GameObject doorCamera;
    private bool focusingOnDoor = false;
    private bool finishedCutscene = false;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (PlayerManager.instance.CompanionMode && !targets.Contains(playerTwo.transform))
        {
            targets.Add(playerTwo.transform);
        }
        else if (!PlayerManager.instance.CompanionMode && targets.Contains(playerTwo.transform))
        {
            targets.Remove(playerTwo.transform);
        }

        if (targets.Count == 0) { return; }

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, newPosition, ref velocity, smoothTime);

        if(PressurePlateManager.instance.allPlatesPressed && !finishedCutscene)
        {
            FocusOnDoor();
            finishedCutscene = true;
        }
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

            Debug.Log("Bounds x: " + bounds.size.x);
            Debug.Log("Bounds y: " + bounds.size.y);
            Debug.Log("Bounds z: " + bounds.size.z);

            // Check to see if the any of the bounds are out of the camera view
            if (bounds.size.x > 50 || bounds.size.y > 25 || bounds.size.z > 50 && !focusingOnDoor)
            {
                // mainCamera.SetActive(false);
                if (targets.Contains(PlayerManager.instance.playerTwo.transform) && targets.Contains(PlayerManager.instance.playerOne.transform))
                {
                    // Activate separate cameras
                    playerOneCamera.SetActive(true);
                    playerTwoCamera.SetActive(true);

                    // Remove players from the targets list
                    targets.Remove(PlayerManager.instance.playerOne.transform);
                    targets.Remove(PlayerManager.instance.playerTwo.transform);
                }
            }
            else if (bounds.size.x < 50 || bounds.size.y < 25 || bounds.size.z < 50 && !focusingOnDoor)
            {
                // mainCamera.SetActive(true);
                if (!targets.Contains(playerTwo.transform) && !targets.Contains(playerOne.transform))
                {
                    // Add players to the targets list
                    targets.Add(playerOne.transform);
                    targets.Add(playerTwo.transform);
                }
                // Deactivate separate cameras
                playerOneCamera.SetActive(false);
                playerTwoCamera.SetActive(false);
            }
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
