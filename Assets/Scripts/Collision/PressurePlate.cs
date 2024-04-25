using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isPressed;

    void OnCollisionEnter(Collision other)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (other.gameObject.tag == "Player" && !isPressed)
        {
            Debug.Log("Player stepped on pressure plate");
            GetComponent<AudioSource>().Play();
            transform.localScale = new Vector3(transform.localScale.x, 0.15f, transform.localScale.z);
            isPressed = true;

            // GetComponent<Collider>().enabled = false;
            // Material remover curerntly not working
            // Material[] materials = new Material[renderer.materials.Length - 1];

            // for (int i = 0; i < materials.Length; i++)
            // {
            //     materials[i-1] = renderer.materials[i];
            // }
            // renderer.materials = materials;

            PressurePlateManager.instance.PlatePressed();
        }
    }
}