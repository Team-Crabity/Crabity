using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressurePlateManager : MonoBehaviour
{
    // Make the class a singleton
    public static PressurePlateManager instance { get; private set;}

    public static int totalPlates;
    public static int platesPressed;
    public bool allPlatesPressed;

    [SerializeField]
    List<GameObject> doors;

    // Make sure only one manager exists
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        totalPlates = FindObjectsOfType<PressurePlate>().Length + FindObjectsOfType<TogglePressurePlate>().Length;
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

    private void CloseDoor() {
        for (int i = 0; i < doors.Count; i += 1) {
            doors[i].GetComponent<Animator>().SetBool("character_nearby", false);
        }
    }

    public void TogglePlate(bool pressed) {
        if (pressed) {
            PlatePressed();
            return;
        }
        Debug.Log("Toggle Pressure plate was un-pressed.");
        platesPressed--;
        if (platesPressed != totalPlates) {
            CloseDoor();
        }
    }
}