using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCollision : MonoBehaviour
{
    private Rigidbody rb1;
    private Rigidbody rb2;
    [SerializeField] private List<AudioClip> SoundList;
    private AudioSource glassBreak;

    private BoxCollider[] boxColliders;

    void Start()
    {
        rb1 = PlayerManager.instance.playerOne.GetComponent<Rigidbody>();
        rb2 = PlayerManager.instance.playerTwo.GetComponent<Rigidbody>();
        glassBreak = GetComponent<AudioSource>();

        // Make sure all triggers are on
        boxColliders = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider boxCollider in boxColliders) {
            boxCollider.isTrigger = true;
        }
    }

    void Update()
    {
        // Debug.Log("P1 Velocity: " + rb1.velocity.magnitude);
        // if(PlayerManager.instance.CompanionMode)
        // {
        //     Debug.Log("P2 Velocity: " + rb2.velocity.magnitude);
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        BoxCollider[] boxColliders = GetComponents<BoxCollider>();

        if ((other.gameObject == PlayerManager.instance.playerOne && rb1.velocity.magnitude > 20)||  
        (other.gameObject == PlayerManager.instance.playerTwo && rb2.velocity.magnitude > 20))
        {
            Debug.Log("Player collided with glass at high velocity");
            glassBreak.clip = SoundList[Random.Range(0,SoundList.Count)];
            glassBreak.Play();
            foreach (BoxCollider boxCollider in boxColliders)
            {
                boxCollider.enabled = false;
            }
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    // BoxCollider[] boxColliders = GetComponents<BoxCollider>();

    // if (PlayerManager.instance.CompanionMode)
    // {
    //     Debug.Log("P1 Velocity: " + rb1.velocity.magnitude);
    //     Debug.Log("P2 Velocity: " + rb2.velocity.magnitude);
    //     if (rb1.velocity.magnitude > 15 && rb2.velocity.magnitude > 15) 
    //     {
    //         Debug.Log("Both players collided with glass");
    //         if (other.gameObject.tag == "Player")
    //         {
    //             Debug.Log("P1 Velocity: " + rb1.velocity.magnitude);
    //             Debug.Log("P2 Velocity: " + rb2.velocity.magnitude);
    //             Debug.Log("Both players collided with glass at high velocity");
    //             glassBreak.Play();
    //             foreach (BoxCollider boxCollider in boxColliders)
    //             {
    //                 boxCollider.enabled = false;
    //             }
    //             gameObject.GetComponent<MeshRenderer>().enabled = false;
    //         }
    //     }
    // }
    // else if (!PlayerManager.instance.CompanionMode)
    // {
    //     if (other.gameObject == PlayerManager.instance.playerOne 
    //     && rb1.velocity.magnitude > 20) 
    //     {
    //         Debug.Log("P1 Velocity: " + rb1.velocity.magnitude);
    //         Debug.Log("Player 1 collided with glass at high velocity");
    //         glassBreak.Play();
    //         foreach (BoxCollider boxCollider in boxColliders)
    //         {
    //             boxCollider.enabled = false;
    //         }
    //         gameObject.GetComponent<MeshRenderer>().enabled = false;
    //     }
    // }
}
