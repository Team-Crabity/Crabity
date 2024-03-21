using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public float delay = 0.5f;
    private AudioSource doorSound;

    void Start()
    {
        doorSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        doorSound.Play();
        StartCoroutine(Delayed());
    }

    public IEnumerator Delayed()
    {
        yield return new WaitForSeconds(delay);
        Scene s = SceneManager.GetActiveScene();
        Debug.Log("Going to scene: " + (s.buildIndex + 1));
        SceneManager.LoadScene(s.buildIndex + 1);
    }

}
