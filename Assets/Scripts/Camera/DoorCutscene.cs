using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCutscene : MonoBehaviour
{
    [Header("Door")]
    public GameObject doorCamera;
    private bool focusingOnDoor = false;
    private bool finishedCutscene = false;
    [SerializeField]
    private float duration = 3.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (PressurePlateManager.instance.allPlatesPressed && !finishedCutscene)
        {
            FocusOnDoor();
        }
    }

    private void FocusOnDoor()
    {
        StartCoroutine(FocusSequence());
    }

    private IEnumerator FocusSequence()
    {
        // Start cutscene on the door for 3 sec
        focusingOnDoor = true;
        doorCamera.SetActive(true);
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(duration);

        finishedCutscene = true;
        focusingOnDoor = false;
        doorCamera.SetActive(false);
        Debug.Log("Door camera sequence complete");
    }
}
