using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Movement movement;
    private SwitchGravity switchGravity;

    void Start()
    {
        movement = GetComponent<Movement>();
        switchGravity = GetComponent<SwitchGravity>();
    }

    void FixedUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (movement == null || switchGravity == null)
            return;

        // Check if gravity switch modifier keys are held down
        bool isOnMac = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX;
        bool rightHeld = isOnMac ? Input.GetKey(KeyCode.RightAlt) : Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightControl);
        bool leftHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        if (rightHeld || leftHeld)
        {
            switchGravity.ChangeGravity();
        }
        else if (!rightHeld || !leftHeld)
        {
            movement.MovePlayer();
        }
    }
}

