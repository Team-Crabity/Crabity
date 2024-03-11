using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    
    public float amp;
    public float freq;
    public bool isPickedUp;
    public float smoothTime;

    Vector3 initPos;

    private Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            Vector3 offset = new Vector3(-22, -5, -34.5f);
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
        } else {
            transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, initPos.z);
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
