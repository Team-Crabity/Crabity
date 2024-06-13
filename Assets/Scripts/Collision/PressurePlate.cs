using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isPressed;
    public Material baseColor;
    [SerializeField] private List<AudioClip> SoundList;
    private AudioSource Source;

    void Start()
    {
        Source = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        Renderer renderer = GetComponent<Renderer>();

        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Block") && !isPressed)
        {
            Debug.Log("Player stepped on pressure plate");
            Source.clip = SoundList[Random.Range(0,SoundList.Count)];
            Source.Play();
            transform.localScale = new Vector3(transform.localScale.x, 0.15f, transform.localScale.z);
            isPressed = true;
            // Change the pressure plate material element 0 to base color
            if (renderer.materials.Length > 0)
            {
                Debug.Log("Changing material color");
                renderer.materials[0] = baseColor;
            }

            // Disable ArrowMapSprite in the children
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            GameObject.FindObjectOfType<PressurePlateManager>().PlatePressed();
        }
    }
}