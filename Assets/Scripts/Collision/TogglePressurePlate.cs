using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TogglePressurePlate : MonoBehaviour
{
    private bool onPlate;
    public Material baseColor;

    // Keep track of scale
    private Vector3 originalScale;
    private Vector3 pressedScale;

    // Store audio source
    private AudioSource audioSource;

    // Keep track of pressure plate cooldown
    private float pressurePlateCooldown = 1.5f;
    private bool isScaling = false;

    void Start() {
        originalScale = transform.localScale;
        pressedScale = new Vector3(transform.localScale.x, 0.25f, transform.localScale.z);
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (!isScaling) {
            Scale();
        }
    }

    void OnCollisionStay(Collision other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Block" || other.gameObject.tag == "Box") && !onPlate) {
            Debug.Log(other.gameObject.tag + " is on toggle plate");
            onPlate = true;
            audioSource.Play();
            PressurePlateManager.instance.TogglePlate(onPlate);
        }
    }

    void OnCollisionExit(Collision other) {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Block") && onPlate) {
            Debug.Log(other.gameObject.tag + " exited toggle plate");
            onPlate = false;
            PressurePlateManager.instance.TogglePlate(onPlate);
        }
    }

    void Scale() {
        StartCoroutine(ScaleOverTime(pressurePlateCooldown, onPlate ? pressedScale : originalScale));
    }

    IEnumerator ScaleOverTime(float time, Vector3 scale) {
        if (scale == transform.localScale) {
            yield break;
        }

        // Start scaling
        isScaling = true;

        // Get current scale
        Vector3 startScale = transform.localScale;

        float counter = 0;
        while (counter < time) {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, scale, counter / time);
            yield return null;
        }

        // Stop scaling  
        isScaling = false;
    }
}