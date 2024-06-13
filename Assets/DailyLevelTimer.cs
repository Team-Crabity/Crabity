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
        int hoursLeft = (24 - currentTime.Hour);
        int minutesLeft = (60 - currentTime.Minute);
        int secondsLeft = (60 - currentTime.Second);
        if (minutesLeft > 0 && secondsLeft > 0) {
            hoursLeft -= 1;
        }
        tMP_Text.text = "Resets in " + hoursLeft + ":" + minutesLeft + ":" + secondsLeft + " hrs";
    }
}
