using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataCollectionButton : MonoBehaviour
{

    TMP_Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        buttonText.text = PlayerPrefs.GetInt("playerConsent") == 1 ? "On" : "Off";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick() {
        if (buttonText.text == "Off") {
            buttonText.text = "On";
            AnalyticsManager.instance.StartDataCollection();   
        } else {
            buttonText.text = "Off";
            AnalyticsManager.instance.StopDataCollection();
        }
    }
}
