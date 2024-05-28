using UnityEngine;
using System.Collections;

public class LaserTrap : MonoBehaviour
{
    [Header ("Audio")]
    [SerializeField] private AudioClip IdleSound;

    private AudioSource Source;

    [SerializeField] public GameObject laser; // Reference to the existing laser GameObject
    public float laserLifetime = 2.0f; // Duration for which the laser stays active
    public float respawnInterval = 3.0f; // Time interval between each laser respawn

    private Vector3 initialPosition; // Initial position of the laser
    private Quaternion initialRotation; // Initial rotation of the laser

    void Start()
    {
        Source = GetComponent<AudioSource>();
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
            // Activate the laser and reset its position and rotation
            //laser.transform.localPosition = initialPosition;
            //laser.transform.localRotation = initialRotation;
            laser.SetActive(true);
            Source.clip = IdleSound;
            Source.volume = (0.7f);
            Source.Play();
            Source.loop = true;

            // Wait for the laser lifetime duration
            yield return new WaitForSeconds(laserLifetime);

            // Deactivate the laser
            Source.Stop();
            Source.loop = false;
            laser.SetActive(false);

            // Wait for the respawn interval
            yield return new WaitForSeconds(respawnInterval);
        }
    }
}
