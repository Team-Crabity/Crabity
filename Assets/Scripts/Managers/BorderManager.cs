using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderManager : MonoBehaviour
{

    public static BorderManager instance { get; private set;}

    public GameObject borderPrefab;
    private List<GameObject> borders;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        Quaternion y_rot = Quaternion.Euler(0, 90, 0);
        Quaternion x_rot = Quaternion.Euler(90, 0, 0);
        borders = new List<GameObject>
        {
            // Create 6 planes 500 meters away from spawn position

            // front and back borders
            Instantiate(borderPrefab, new Vector3(0, 0, 200), transform.rotation),
            Instantiate(borderPrefab, new Vector3(0, 0, -200), transform.rotation),

            // left and right borders
            Instantiate(borderPrefab, new Vector3(-200, 0, 0), y_rot),
            Instantiate(borderPrefab, new Vector3(200, 0, 0), y_rot),

            // up and down borders
            Instantiate(borderPrefab, new Vector3(0, -200, 0), x_rot),
            Instantiate(borderPrefab, new Vector3(0, 200, 0), x_rot)
        };

        for (int i = 0; i < borders.Count; i += 1) {
            // disable renderer
            borders[i].GetComponentInChildren<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
