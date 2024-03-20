using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSoundController : MonoBehaviour
{
    public float initialVolume = 0.25f;
    public float initialPitch = 1f;
    private Rigidbody rb;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        source.pitch = initialPitch;
        source.volume = initialVolume;
        source.loop = true;

        Debug.Log("Idle Sound");

    }

    void Update()
    {
        if (rb.velocity.magnitude > 0.2f && !source.isPlaying)
        {
            Debug.Log("Moving Sound");
            source.pitch = initialPitch * 2.0f;
            source.volume = initialVolume * 2.0f;
        }
        else if (rb.velocity.magnitude < 0.1f && source.isPlaying)
        {
            source.Stop();
        }
    }
}
