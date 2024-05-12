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

		AskForConsent();
    }

	void AskForConsent()
	{
		// ... show the player a UI element that asks for consent.
        canvas.enabled = true;
	}

    void OnAgree() {
        Debug.Log("User agreed to share data");

        // start collecting data
        ConsentGiven();

        // disable canvas
        canvas.enabled = false;
    }

    void OnDisagree() {
        // disable canvas
        canvas.enabled = false;
        
        // Go to main menu
        MainMenu();
    }

	void ConsentGiven()
	{
		AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Collecting Data from User");
        
        MainMenu();
	}

    void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
