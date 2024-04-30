using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    public static PressurePlateManager instance;

    public static int totalPlates;
    public static int platesPressed;
    public bool allPlatesPressed;

    [SerializeField]
    List<GameObject> doors;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        totalPlates = FindObjectsOfType<PressurePlate>().Length;
    }

    public void PlatePressed()
    {
        platesPressed++;
        Debug.Log("Plate pressed, total remaining: " + (totalPlates - platesPressed));
        if (platesPressed == totalPlates)
        {
            allPlatesPressed = true;
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        for (int i = 0; i < doors.Count; i += 1) {
            doors[i].GetComponent<Animator>().SetBool("character_nearby", true);
        }
    }
}