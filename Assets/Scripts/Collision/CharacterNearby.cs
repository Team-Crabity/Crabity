using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNearby : MonoBehaviour
{

    // Reference of the glass door prefab and its animations
    public GameObject glassDoorPrefab;
    private Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = glassDoorPrefab.gameObject.GetComponent<Animator>();

        // set mesh renderer off
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == PlayerManager.instance.playerOne 
            || other.gameObject == PlayerManager.instance.playerTwo)
        {
            doorAnimator.SetBool("character_nearby", true);
            GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.instance.playerOne 
            || other.gameObject == PlayerManager.instance.playerTwo)
        {
            doorAnimator.SetBool("character_nearby", false);
        }
    }
}
