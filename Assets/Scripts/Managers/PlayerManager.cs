using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public bool CompanionMode;
    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject playerOneSpawnPoint;
    public GameObject playerTwoSpawnPoint;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Set companion mode to player pref setting
        CompanionMode = PlayerPrefs.GetInt("companionMode", 0) == 1;
    }

    void Update()
    {
        // Secret hotkey for companion mode
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleCompanionMode();
        }

        if (CompanionMode)
        {
            playerTwo.SetActive(true);
        }
        else
        {
            playerTwo.SetActive(false);
        }
    }

    public void ToggleCompanionMode()
    {
        CompanionMode = !CompanionMode;
        PlayerPrefs.SetInt("companionMode", CompanionMode == true ? 1 : 0);
    }

    public bool IsPlayerOne(GameObject player)
    {
        return player == playerOne;
    }

    public bool IsPlayerTwo(GameObject player)
    {
        return player == playerTwo;
    }
}
