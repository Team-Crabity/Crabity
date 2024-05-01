using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    [Header("Gravity")]
    public float gravityScale = 5.0f;
    public float gravity = 9.81f;

    private Vector3 gravityDirection = Vector3.zero;
    public Vector3 newGravity { get; private set; }

    // Keymaps for players to be able to switch gravity
    private Dictionary<KeyCode, Vector3> playerOneKeyMap = new Dictionary<KeyCode, Vector3>
    {
        { KeyCode.W, Vector3.up },
        { KeyCode.A, Vector3.left },
        { KeyCode.S, Vector3.down },
        { KeyCode.D, Vector3.right }
    };
    private Dictionary<KeyCode, Vector3> playerTwoKeyMap = new Dictionary<KeyCode, Vector3>
    {
        { KeyCode.O, Vector3.up },
        { KeyCode.K, Vector3.left },
        { KeyCode.L, Vector3.down },
        { KeyCode.Semicolon, Vector3.right }
    };

    private bool playerOne;
    private bool playerTwo;
    private bool isGrounded;

    void Start()
    {
        playerOne = PlayerManager.instance.IsPlayerOne(gameObject);
        playerTwo = PlayerManager.instance.IsPlayerTwo(gameObject);
        UpdateGravity(Vector3.down);
    }

    void Update()
    {
        isGrounded = gameObject.GetComponent<Movement>().isGrounded;
        // Check if player is on mac
        bool isOnMac = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX;
        bool rightHeld = isOnMac ? Input.GetKey(KeyCode.RightAlt) : Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightControl);
        bool leftHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        if (playerOne && leftHeld)
        {
            ChangeGravity();
        }
        
        if (playerTwo && rightHeld)
        {
            ChangeGravity();
        }
    }

    private void FixedUpdate()
    {
        if (gravityDirection != Vector3.zero)
        {
            UpdateGravity(gravityDirection);
            gravityDirection = Vector3.zero;
        }
    }

    private void ChangeGravity()
    {
        if (!isGrounded) return;

        // Determine which keymap to use based on player selection
        Dictionary<KeyCode, Vector3> currentKeyMap = playerOne ? playerOneKeyMap : playerTwoKeyMap;

        // Check key presses and update grav direction
        foreach (var entry in currentKeyMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                gravityDirection = entry.Value;

                RotatePlayer(PlayerManager.instance.playerOne);
                RotatePlayer(PlayerManager.instance.playerTwo);

                isGrounded = false;
                break;
            }
        }
    }

    private void RotatePlayer(GameObject player)
    {
        // Rotate the player to match the new gravity direction
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, -gravityDirection) * player.transform.rotation;
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 1.0f);
    }

    private void UpdateGravity(Vector3 direction)
    {
        Vector3 newGravity = direction * gravity * gravityScale;
        Physics.gravity = newGravity;
    }
}