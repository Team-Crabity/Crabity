using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public bool CompanionMode;
    public GameObject playerOne;
    public GameObject playerTwo;

    private RotateObject playerOneRotate;
    private SwitchGravity playerOneGravity;
    private SwitchGravity playerTwoGravity;
    private MultipleTargetCamera cameraScript;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        cameraScript = Camera.main.GetComponent<MultipleTargetCamera>();
        
        if (playerOne != null)
        {
            playerOneRotate = playerOne.GetComponentInParent<RotateObject>();
            playerOneGravity = playerOne.GetComponent<SwitchGravity>();
        }
        else if (playerTwo != null)
        {
            playerTwoGravity = playerTwo.GetComponent<SwitchGravity>();
        }
    }

    void Update()
    {
        Transform playerTwoChild = playerTwo.transform.Find("robotSphere").gameObject.transform;
        if (CompanionMode)
        {
            playerTwo.SetActive(true);
            // Disable player one gravity switching in companion mode
            if (playerOneGravity != null)
            {
                playerOneGravity.playerOne = true;
            }

            // Enable player two gravity switching in companion mode
            if (playerTwoGravity != null)
            {
                playerTwoGravity.playerTwo = true;
                playerTwoGravity.enabled = true;
            }
            
            // Add player two's child to camera script list
            if (cameraScript != null && !cameraScript.targets.Contains(playerTwoChild))
            {
                cameraScript.targets.Add(playerTwoChild);
            }
        }
        else
        {
            playerTwo.SetActive(false);

            // Remove player two's child from camera script list
            if (cameraScript != null && cameraScript.targets.Contains(playerTwoChild))
            {
                cameraScript.targets.Remove(playerTwoChild);
            }

            // Allow both functionality for player one when not in companion mode
            if (playerOneGravity != null)
            {
                playerOneGravity.enabled = true;
            }

            if (playerOneRotate != null)
            {
                playerOneRotate.enabled = true;
            }
        }
    }
}
