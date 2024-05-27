using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    // canvas
    Canvas canvas;

    // buttons
    Button[] buttons;

    public static AnalyticsManager instance {get; private set;}

    void Awake() {
        DontDestroyOnLoad(gameObject);

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("playerConsent")) {
            MainMenu();
        }
    }

    async void Start()
    {

        // disable canvas element
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;

        // get buttons and assign callbacks
        buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].GetComponentInChildren<TMP_Text>().text == "Yes") {
                buttons[i].onClick.AddListener(OnAgree);
            } else {
                buttons[i].onClick.AddListener(OnDisagree);
            }
        }

		await UnityServices.InitializeAsync();

        int consent = PlayerPrefs.GetInt("playerConsent", -1);
        if (consent == -1) {
            AskForConsent();
        } else if (consent == 1) {
            StartDataCollection();
        }
    }

	void AskForConsent()
	{
		// ... show the player a UI element that asks for consent.
        canvas.enabled = true;
	}

    void OnAgree() {
        // disable canvas
        canvas.enabled = false;
        
        Debug.Log("User agreed to share data");
        StartDataCollection();
        MainMenu();
    }

    void OnDisagree() {
        // disable canvas
        canvas.enabled = false;

        StopDataCollection();
        MainMenu();
    }

    void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StopDataCollection() {
        AnalyticsService.Instance.StopDataCollection();
        Debug.Log("Stopped collecting Data from User");
        PlayerPrefs.SetInt("playerConsent", 0);
        Debug.Log("PLAYER CONSENT: " + PlayerPrefs.GetInt("playerConsent"));
    }

    public void StartDataCollection() {
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Started collecting Data from User");
        PlayerPrefs.SetInt("playerConsent", 1);
        Debug.Log("PLAYER CONSENT: " + PlayerPrefs.GetInt("playerConsent"));
    }
}
