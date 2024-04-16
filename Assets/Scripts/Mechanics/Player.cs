using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        gameObject.transform.position = spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the border
        if (collision.gameObject.tag == "Border")
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        // Reset position to the spawn point
        transform.position = spawnPoint;
    }
}
