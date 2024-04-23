using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    //TODO: Change camera to adaptive split screen instead of regular camera
    //CREDITS: Brackeys - https://www.youtube.com/watch?v=aLpixrPvlB8
    [Header("Players to Track")]
    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;

    [Header("Player Game Objects")]
    public GameObject playerOne;
    public GameObject playerTwo;

    [Header("Player Cameras")]
    public GameObject playerOneCamera;
    public GameObject playerTwoCamera;

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

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
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
            if(bounds.size.x > 60 || bounds.size.y > 30 || bounds.size.z > 45)
            {
                
                if(targets.Contains(playerTwo.transform))
                {
                    // Activate separate cameras
                    playerOneCamera.SetActive(true);
                    playerTwoCamera.SetActive(true);

                    // Remove player two from the targets list
                    targets.Remove(playerTwo.transform);
                }
            }
            else if (bounds.size.x < 60 || bounds.size.y < 30 || bounds.size.z < 45)
            {
                // Currently not working as expected bc of conditional
                if(!targets.Contains(playerTwo.transform))
                {
                    // Add player two to the targets list
                    targets.Add(playerTwo.transform);
                }
                // Deactivate separate cameras
                playerOneCamera.SetActive(false);
                playerTwoCamera.SetActive(false);
            }
        }
        return bounds.center;
    }
}
