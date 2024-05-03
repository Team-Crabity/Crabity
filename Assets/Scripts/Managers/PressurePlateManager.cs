using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    public static int totalPlates;
    public static int platesPressed;
    public bool allPlatesPressed;

    [SerializeField]
    List<GameObject> doors;

    private void Start()
    {
        totalPlates = FindObjectsOfType<PressurePlate>().Length;
        platesPressed = 0;
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