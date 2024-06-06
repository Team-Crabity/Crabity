using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserSeed : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private List<AudioClip> IdleSound;
    private AudioSource Source;
    private EDAudio EDAudioScript;

    [SerializeField] public GameObject laser; 
    public float minLaserLifetime = 1.0f;
    public float maxLaserLifetime = 3.0f; 
    public float minRespawnInterval = 2.0f;
    public float maxRespawnInterval = 5.0f; 
    public float animationDuration = 0.15f; 

    private System.Random random; // Random number generator

    void Start()
    {
        Source = GetComponent<AudioSource>();
        EDAudioScript = GetComponent<EDAudio>();
        random = new System.Random(); // Initialize the random number generator with a seed

        if (laser != null)
        {
            StartCoroutine(ManageLaser());
        }
        else
        {
            Debug.LogError("Laser GameObject is not assigned.");
        }
    }

    IEnumerator ManageLaser()
    {
        while (true)
        {
            float laserLifetime = RandomRange(random, minLaserLifetime, maxLaserLifetime);
            float respawnInterval = RandomRange(random, minRespawnInterval, maxRespawnInterval);

            // Activate laser
            // laser.SetActive(true);
            StartCoroutine(AnimateLaser(Vector3.zero, Vector3.one, animationDuration));
            if (Source.volume >= 0.35)
            {
                Source.clip = IdleSound[Random.Range(0, IdleSound.Count)];
                Source.Play();
            }

            yield return new WaitForSeconds(laserLifetime);

            StartCoroutine(AnimateLaser(new Vector3(0, -3.2f, 0), new Vector3(1, 0, 1), animationDuration));

            yield return new WaitForSeconds(respawnInterval);
        }
    }

    IEnumerator AnimateLaser(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 initialPosition = laser.transform.localPosition;
        Vector3 initialScale = laser.transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            laser.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            laser.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        // Make sure final position and scale are set
        laser.transform.localPosition = targetPosition;
        laser.transform.localScale = targetScale;
    }

    // Helper method to generate a random float in a range using System.Random
    private float RandomRange(System.Random random, float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}
