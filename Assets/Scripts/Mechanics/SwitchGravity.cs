using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchGravity : MonoBehaviour
{
    [Header ("Audio")]
    [SerializeField] private List<AudioClip> GravitySounds;
    private AudioSource Source;

    [Header ("CooldownUI")]
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;

    [Header("Gravity")]
    public float gravityScale = 5.0f;
    public float gravity = 9.81f;

    [Header("Movement Script")]
    [SerializeField] private Movement movement;

    private Vector3 gravityDirection = Vector3.zero;
    public Vector3 newGravity { get; private set; }

    // Keep track of number of gravity switches
    public int gravitySwitchCount = 0;

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
        { KeyCode.UpArrow, Vector3.up },
        { KeyCode.LeftArrow, Vector3.left },
        { KeyCode.DownArrow, Vector3.down },
        { KeyCode.RightArrow, Vector3.right }
    };

    private bool playerOne;
    private bool playerTwo;
    private bool isGrounded;
    private bool gravityOnCooldown;

    //Cooldown UI Script
    // [SerializeField] private CoolDown cooldown;

    void Start()
    {
        Source = GetComponent<AudioSource>();

        playerOne = PlayerManager.instance.IsPlayerOne(gameObject);
        playerTwo = PlayerManager.instance.IsPlayerTwo(gameObject);
        UpdateGravity(Vector3.down);

        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    void Update()
    {
        // float timer = cooldown._nextFireTime - Time.time;
        // if (!cooldown.IsCoolingDown) 
        // {
        //     textCooldown.gameObject.SetActive(false);
        // }
        // else if (timer > 0)
        // {
        //     textCooldown.text = Mathf.RoundToInt(timer).ToString();
        //     imageCooldown.fillAmount = timer / cooldown.cooldownTime; 
        // }
    }

    private void FixedUpdate()
    {
        isGrounded = movement.IsGrounded();
        gravityOnCooldown = movement.gravityOnCooldown;
        if (gravityDirection != Vector3.zero)
        {
            UpdateGravity(gravityDirection);
            gravityDirection = Vector3.zero;
        }

    }

    public void ChangeGravity()
    {
        if (gravityOnCooldown) return;
        // Determine which keymap to use based on player selection
        Dictionary<KeyCode, Vector3> currentKeyMap = PlayerManager.instance.CompanionMode ? playerTwoKeyMap : playerOneKeyMap;

        // Check key presses and update grav direction
        foreach (var entry in currentKeyMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                gravityOnCooldown = true;
                // if (cooldown.IsCoolingDown) return;

                gravityDirection = entry.Value;

                RotatePlayer(PlayerManager.instance.playerOne);
                RotatePlayer(PlayerManager.instance.playerTwo);

                isGrounded = false;

                //Cooldown UI
                // cooldown.StartCooldown();
                // textCooldown.gameObject.SetActive(true);

                // Update gravity switch count
                gravitySwitchCount += 1;

                //Play Random Grav sound
                AudioClip clip = null;
                clip = GravitySounds[UnityEngine.Random.Range(0,GravitySounds.Count)];
                Source.clip = clip;
                Source.volume = (0.7f);
                Source.Play();

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