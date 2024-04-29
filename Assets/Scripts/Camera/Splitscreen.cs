using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitscreen : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject playerOneCamera;
    public GameObject playerTwoCamera;
    public bool isSplitScreen = false;

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
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }
}
