using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySounds : MonoBehaviour
{
    private AudioSource Source;
    public List<AudioClip> GravityClips;

    void Start()
    {
        Source = GetComponent<AudioSource>();
    }
    
    public void PlayGravitySound()
    {
        AudioClip clip = null;
        clip = GravityClips[UnityEngine.Random.Range(0, GravityClips.Count)];
        Source.clip = clip;
        Source.volume = (0.7f);
        Source.pitch = Random.Range(0.9f, 1.1f);
        Source.Play();
    }
}
