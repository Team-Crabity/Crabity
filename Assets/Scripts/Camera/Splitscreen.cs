using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitscreen : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject playerOneCamera;
    public GameObject playerTwoCamera;

    [Header("Split Screen")]
    public bool isSplitScreen = false;

    [Header("Screen Bounds")]
    public float xMin = 0.05f;
    public float xMax = 0.95f;
    public float yMin = 0.2f;
    public float yMax = 0.8f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerManager.instance.CompanionMode)
        {
            if ((OutOfBounds(PlayerManager.instance.playerOne.transform)))
            {
                ToggleSplitScreen(true);
            }
            else
            {
                ToggleSplitScreen(false);
            }
        }
        else
        {
            ToggleSplitScreen(false);
        }
    }

    private void ToggleSplitScreen(bool toggle)
    {
        // Disable or enable both player cameras
        playerOneCamera.SetActive(toggle);
        playerTwoCamera.SetActive(toggle);
        isSplitScreen = toggle;
    }

    private bool OutOfBounds(Transform target)
    {
        // Check if target is out of bounds of the main camera
        Vector3 screenPoint = mainCamera.GetComponent<Camera>().WorldToViewportPoint(target.position);
        return screenPoint.x < xMin || screenPoint.x > xMax || screenPoint.y < yMin || screenPoint.y > yMax;
    }
}
