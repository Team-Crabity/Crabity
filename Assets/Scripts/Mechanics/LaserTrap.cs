using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserTrap : MonoBehaviour
{
    [Header ("Audio")]
    [SerializeField] private List<AudioClip> IdleSound;
    private AudioSource Source;
    private EDAudio EDAudioScript;

    [SerializeField] public GameObject laser; // Reference to the existing laser GameObject
    public float laserLifetime = 2.0f; // Duration for which the laser stays active
    public float respawnInterval = 3.0f; // Time interval between each laser respawn
    public float animationDuration = 0.15f; // Duration for the laser to animate

    private Vector3 initialPosition; // Initial position of the laser
    private Quaternion initialRotation; // Initial rotation of the laser

    void Start()
    {
        Source = GetComponent<AudioSource>();
        EDAudioScript = GetComponent<EDAudio>();
        if (laser != null)
        {
            //un comment position variables if things break later
            //initialPosition = laser.transform.localPosition;
            //initialRotation = laser.transform.localRotation;
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
            // Activate laser
            // laser.SetActive(true);
            StartCoroutine(AnimateLaser(Vector3.zero, Vector3.one, animationDuration));
            if (Source.volume >= 0.35)
            {
                Source.clip = IdleSound[Random.Range(0,IdleSound.Count)];
                Source.Play();
            }

            // Wait for the laser lifetime duration
            yield return new WaitForSeconds(laserLifetime);

            // Deactivate the laser + moves it to hidden
            // laser.SetActive(false);
            StartCoroutine(AnimateLaser(new Vector3(0, -3.2f, 0), new Vector3(1, 0, 1), animationDuration));


            // Wait for the respawn interval
            yield return new WaitForSeconds(respawnInterval);
        }
    }
    
    IEnumerator AnimateLaser(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 initialPosition = laser.transform.localPosition;
        Vector3 intialScale = laser.transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time/duration;
            laser.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            laser.transform.localScale = Vector3.Lerp(intialScale, targetScale, t);
            yield return null;
        }

        // Make sure final pos and scale are set
        laser.transform.localPosition = targetPosition;
        laser.transform.localScale = targetScale;
    }
}
