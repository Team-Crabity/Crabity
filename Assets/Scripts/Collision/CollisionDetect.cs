using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    public GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Laser") {
            transform.position = spawnPoint.transform.position;
        }
    }
}
