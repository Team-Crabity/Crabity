using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    
    [SerializeField]
    private GameObject border;
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private float duration = 0.1f;
    [SerializeField]
    private GameObject mapCamera;

    private float targetWidth;
    private float currentWidth;
    private bool mapOpening = false;
    [SerializeField]
    private RotateObject rotateObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Toggle map on and off with slerp effect
            ToggleMap();
        }       
    }

    public void ToggleMap()
    {
        StartCoroutine(SlerpMap());
    }

    public IEnumerator SlerpMap()
    {
        if (map == null)
        {
            map = GameObject.Find("Map");
        }

        if(!mapOpening && !rotateObject.isRotating())
        {
            mapCamera.SetActive(!mapCamera.activeSelf);
            mapOpening = true;
            // Set map camera to active to to update momentarily
            // mapCamera.SetActive(true);
            targetWidth = map.transform.localScale.x == 0 ? 1 : 0;

            float time = 0;
            float currentWidth = map.transform.localScale.x;
            
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                float newWidth = Mathf.Lerp(currentWidth, targetWidth, t);
                // Map pop in horizontally
                map.transform.localScale = new Vector3(newWidth, newWidth, map.transform.localScale.z);
                // Border pop in vertically
                border.transform.localScale = new Vector3(border.transform.localScale.x, newWidth, border.transform.localScale.z);
                yield return null;
            }
            // mapCamera.SetActive(false);
            mapOpening = false;
        }
    }
}
