using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    public GameObject spawnPoint;
    private AudioSource Source;
    [SerializeField] private AudioClip RespawnSound;
    [SerializeField] private AudioClip DeathSound;
    void Start()
    {
        transform.position = spawnPoint.transform.position;
        Source = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Laser") {
            if (collision.gameObject.tag == "Laser") {
                StartCoroutine(playDeathSound());
            }
            else {
                Source.clip = RespawnSound;
                Source.pitch = (1f);
                Source.volume = (1f);
                Source.Play();
            }
            transform.position = spawnPoint.transform.position;
        }
    }
    IEnumerator playDeathSound()
        {
            Source.clip = DeathSound;
            Source.pitch = (1f);
            Source.volume = (0.6f);
            Source.Play();
            yield return new WaitForSeconds(0.2f);
            Source.clip = RespawnSound;
            Source.Play();
        }
}
