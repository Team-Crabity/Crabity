using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCutscene : MonoBehaviour
{
    [Header("Door")]
    public GameObject doorCamera;
    private bool finishedCutscene = false;
    [SerializeField]
    private float duration = 3.0f;

    void LateUpdate()
    {
        if (PressurePlateManager.instance.allPlatesPressed && !doorCamera.activeSelf && !finishedCutscene)
        {
            FocusOnDoor();
        }
    }

    // Separate function for FocusOnDoor to be used from other scripts
    public void FocusOnDoor()
    {
        StartCoroutine(FocusSequence());
    }

    private IEnumerator FocusSequence()
    {
        // Start cutscene on the door for 3 sec
        doorCamera.SetActive(true);
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(duration);

        finishedCutscene = true;
        doorCamera.SetActive(false);
        Debug.Log("Door camera sequence complete");
    }
}
