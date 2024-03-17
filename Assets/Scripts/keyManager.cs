using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    public float amp;
    public float freq;
    public bool isPickedUp;
    public float smoothTime;
    public Transform rotateAround;

    Vector3 initPos;

    private RotateObject rotateObject;
    private Vector3 vel;

    void Start()
    {
        initPos = transform.position;
        rotateObject = GetComponentInParent<RotateObject>(); // Get the RotateObject script from the parent obj
        
    }

    void Update()
    {
        if (isPickedUp)
        {
            transform.parent = null;
            Vector3 offset = new Vector3(0, -5, 0);
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
            transform.RotateAround(rotateAround.position, Vector3.up, 90 * Time.deltaTime);  // have object rotate in place
        }
        else
        {
            if (rotateObject != null && !rotateObject.isRotating())
            {
                transform.position += new Vector3(0, Mathf.Sin(Time.time * freq) * amp, 0); // changed to no longer use initPos
                transform.RotateAround(rotateAround.position, Vector3.up, 90 * Time.deltaTime);  // have object rotate in place
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
        }
    }
}
