using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    private AudioSource Source;
    public List<AudioClip> WalkingSounds;
    private bool isJumping;

    void Start() 
    {
        Source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        isJumping = GetComponent<Movement>().isJumping;
    }

    public void PlaySound()
    {
        if(isJumping)
        {
            return;
        }
        AudioClip clip = null;
        clip = WalkingSounds[Random.Range(0,WalkingSounds.Count)];
        Source.clip = clip;
        Source.volume = Random.Range(0.005f, 0.01f);
        Source.pitch = Random.Range(0.25f, 0.35f);
        Source.Play();
    }
}
