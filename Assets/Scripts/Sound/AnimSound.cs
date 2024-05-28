using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSound : MonoBehaviour
{
    private AudioSource Source;
    public List<AudioClip> WalkingSounds;

    void Start() 
    {
        Source = GetComponent<AudioSource>();
    }

    public void playSound()
    {
        AudioClip clip = null;
        clip = WalkingSounds[Random.Range(0,WalkingSounds.Count)];
        Source.clip = clip;
        Source.volume = Random.Range(0.4f, 0.5f);
        Source.pitch = Random.Range(0.95f, 1.05f);
        Source.Play();
    }
}
