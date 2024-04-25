using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    public static PressurePlateManager instance;

    public static int totalPlates;
    public static int platesPressed;
    public static Animator doorAnimator;
    public bool allPlatesPressed;
    public GameObject door;

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
        doorAnimator = door.GetComponent<Animator>();
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
        doorAnimator.SetBool("character_nearby", true);
    }
}