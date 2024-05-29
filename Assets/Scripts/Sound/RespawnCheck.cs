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
        Source = GetComponent<AudioSource>();
        if (!PMScript.CompanionMode) 
        {
            Source.clip = respawnSounds[0];
        }
        else 
        {
            Source.clip = respawnSounds[1];
        }
        Source.volume = (1f);
        Source.Play();
    }
}
