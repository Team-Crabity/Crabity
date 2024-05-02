using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDoorSound : MonoBehaviour
{

    private AudioSource doorAudio;
    void Start()
    {
        doorAudio = GetComponent<AudioSource>();
    }

    public void PlaySound() 
    {
        doorAudio.Play();
    }
}
