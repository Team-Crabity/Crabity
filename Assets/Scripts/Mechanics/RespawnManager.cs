using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    Transform[] borders;

    // Start is called before the first frame update
    void Start()
    {
        // Get borders
        borders = GetComponentsInChildren<Transform>();

        // Turn off the mesh renderers of the borders
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i += 1)
        {
            // Disable mesh
            meshRenderers[i].enabled = false;

            // Tag the border for checking
            borders[i].gameObject.tag = "Border";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
