using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenPlate : MonoBehaviour
{
    private bool isPressed;
    public Material baseColor;
    [SerializeField] private List<AudioClip> SoundList;
    private AudioSource Source;

    // Unique identifier for each pressure plate
    public string plateID;

    void Start()
    {
        Source = GetComponent<AudioSource>();

        // Log debug information for all pressure plates and doors at the start
       // LogAllDoorsAndPlates();
    }
    /*
    void LogAllDoorsAndPlates()
    {
        Debug.Log("Pressure Plate ID: " + plateID);
        UnlockableDoor[] doors = FindObjectsOfType<UnlockableDoor>();
        Debug.Log("Number of doors found: " + doors.Length);
        int doorIndex = GetDoorIndex();
        Debug.Log("Corresponding Door Index: " + doorIndex);
    }
    */


    int GetDoorIndex()
    {
        ProcGenPlate[] plates = FindObjectsOfType<ProcGenPlate>();
        System.Array.Sort(plates, (a, b) => a.plateID.CompareTo(b.plateID));
        for (int i = 0; i < plates.Length; i++)
        {
            if (plates[i] == this)
            {
                return i;
            }
        }

        return -1;
    }

    void OnCollisionEnter(Collision other)
    {
        Renderer renderer = GetComponent<Renderer>();

        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Block") && !isPressed)
        {
            Debug.Log("Player stepped on pressure plate");

            Debug.Log("Plate ID: " + plateID);

            Source.clip = SoundList[Random.Range(0, SoundList.Count)];
            Source.Play();
            transform.localScale = new Vector3(transform.localScale.x, 0.15f, transform.localScale.z);
            isPressed = true;
            int doorIndex = GetDoorIndex();
            if (doorIndex >= 0)
            {
                //Debug.Log("Changing door with index: " + doorIndex);
                UnlockableDoor[] doors = FindObjectsOfType<UnlockableDoor>();
                if (doorIndex < doors.Length)
                {
                    // Unlock the corresponding door
                    doors[doorIndex].SetCharacterNearby(true);
                }
            }

            if (renderer.materials.Length > 0)
            {
                Debug.Log("Changing material color");
                renderer.materials[0] = baseColor;
            }
        }
    }
}
