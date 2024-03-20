using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSound : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource source;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        source.pitch = 0.1f;
        source.volume = 0.05f;
        source.loop = true;
    }

    private void Update()
    {
        // Check if the player is moving
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if (isFadingOut)
            {
                StopAllCoroutines(); // Stops the fade out if it's happening
                isFadingOut = false;
            }
            if (!source.isPlaying)
            {
                source.Play();
            }
            StartCoroutine(Fade(true, source, 0.2f, 0.1f, 0.2f)); // Start fade in
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (isFadingIn)
            {
                StopAllCoroutines(); // Cut the fade short
                isFadingIn = false;
            }
            StartCoroutine(Fade(false, source, 1f, 0.05f, 0.1f)); // Start fade out
        }
    }

    public IEnumerator Fade(bool fadeIn, AudioSource source, float duration, float targetVolume, float targetPitch)
    {
        if (fadeIn) isFadingIn = true; else isFadingOut = true;

        float startVol = source.volume;
        float startPitch = source.pitch;
        float time = 0f;

        while (time < duration)
        {
            // string fadeSituation = fadeIn ? "fadeIn" : "fadeOut";
            // Debug.Log(fadeSituation);
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVolume, time / duration);
            source.pitch = Mathf.Lerp(startPitch, targetPitch, time / duration);
            yield return null;
        }

        if (fadeIn) isFadingIn = false; else isFadingOut = false;
    }
}
