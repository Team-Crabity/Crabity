using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class Door : MonoBehaviour
{
    public float delay = 0.5f;
    private AudioSource doorSound;

    void Start()
    {
        doorSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        doorSound.Play();
        StartCoroutine(Delayed());
    }

    public IEnumerator Delayed()
    {
        yield return new WaitForSeconds(delay);
        Scene s = SceneManager.GetActiveScene();
        Debug.Log("Going to scene: " + (s.buildIndex + 1));

        // Set level as complete in player preferences
        PlayerPrefs.SetInt(s.name, 1);
        
        // send level complete
        CustomEvent myEvent = new CustomEvent("levelComplete")
        {
            {"levelName", s.name},
        };
        AnalyticsService.Instance.RecordEvent(myEvent);

        // send number of gravity switches
        CustomEvent gravitySwitchesEvent = new CustomEvent("gravitySwitch") {
            {"levelName", s.name},
            {"gravitySwitchCount", PlayerManager.instance.playerOne.GetComponent<Movement>().gravitySwitchCount},
            {"playerNum", 1},
        };
        AnalyticsService.Instance.RecordEvent(gravitySwitchesEvent);

        // send number of rotations
        CustomEvent perspectiveSwitchEvent = new CustomEvent("perspectiveSwitch") {
            {"levelName", s.name},
            {"switchCount", RotateObject.numRotations},
            {"playerNum", 1},
        };
        AnalyticsService.Instance.RecordEvent(perspectiveSwitchEvent);
        Debug.Log(RotateObject.numRotations);
        
        // Go to next level
        SceneManager.LoadScene(s.buildIndex + 1);
    }
}
