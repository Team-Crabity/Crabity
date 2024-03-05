using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Change Scenes
        Scene s = SceneManager.GetActiveScene();
        Debug.Log("Going to scene: " + (s.buildIndex + 1));
        SceneManager.LoadScene(s.buildIndex + 1);
    }
}
