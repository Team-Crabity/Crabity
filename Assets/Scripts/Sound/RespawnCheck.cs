using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheck : MonoBehaviour
{
    public PlayerManager PMScript;
    public List<AudioClip> respawnSounds;
    private AudioSource Source;
    void Start()
    {
        AudioClip clip = null;
        Source = GetComponent<AudioSource>();
        if (PMScript.CompanionMode) 
        {
            clip = respawnSounds[0];
        }
        else 
        {
            clip = respawnSounds[1];
        }
        Source.volume = (1f);
        Source.Play();
    }
}
