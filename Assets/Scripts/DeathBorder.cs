using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBorder : MonoBehaviour
{
    [Header("Player One")]
    public Rigidbody rb;

    /*
    TODO:
    Create a death border; on collision causes player to respawn back at spawn
    point with a temporary platform beneath them
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
        }
    }
}
