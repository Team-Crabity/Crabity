using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyLevelTimer : MonoBehaviour
{
    private TMP_Text tMP_Text;

    // Start is called before the first frame update
    void Start()
    {
        tMP_Text = GetComponent<TMP_Text>();
        DateTime currentTime = DateTime.UtcNow;
        Debug.Log(24 - currentTime.Hour);
    }

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.UtcNow;
        tMP_Text.text = "Resets in " + (24 - currentTime.Hour) + ":" + (60 - currentTime.Minute) + ":" + 
        (60 - currentTime.Second) + " hrs";
    }
}
