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

    [Header("Camera")]
    public Camera mainCamera;

    private Vector3 velocity;

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


}
