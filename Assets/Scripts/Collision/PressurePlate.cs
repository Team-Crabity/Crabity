using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isPressed;
    public Material baseColor;

    void OnCollisionEnter(Collision other)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (other.gameObject.tag == "Player" && !isPressed)
        {
            Debug.Log("Player stepped on pressure plate");
            GetComponent<AudioSource>().Play();
            transform.localScale = new Vector3(transform.localScale.x, 0.15f, transform.localScale.z);
            isPressed = true;
            // Change the pressure plate material element 0 to base color
            if (renderer.materials.Length > 0)
            {
                Debug.Log("Changing material color");
                renderer.materials[0] = baseColor;
            }

            PressurePlateManager.instance.PlatePressed();
        }
    }
}