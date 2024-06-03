using UnityEngine;
using TMPro;
using System;

public class TimeTracker : MonoBehaviour
{
    public TextMeshProUGUI liveTimeText; // Reference to the TMP Text component for live updates
    public TextMeshProUGUI finalTimeText; // Reference to the TMP Text component for the final time
    private float elapsedTime;
    private bool isTracking;

    void Start()
    {
        elapsedTime = 0f;
        isTracking = true;
    }

    void Update()
    {
        if (isTracking)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    private void UpdateTimeDisplay()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        string formattedTime = string.Format("Time: {0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        if (liveTimeText != null)
        {
            liveTimeText.text = formattedTime;
        }
    }

    public void StopTracking()
    {
        isTracking = false;
        UpdateFinalTimeDisplay();
    }

    private void UpdateFinalTimeDisplay()
    {
        if (finalTimeText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            finalTimeText.text = string.Format("Time: {0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public string GetFormattedElapsedTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        return string.Format("Time: {0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
    }
}
