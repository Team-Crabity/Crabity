using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class ChangeScene : MonoBehaviour
{
    public bool ChangeTheScene = false;
    public GameObject Cam;
    private ObjClick ObjClickScript;
    public GameObject Posi;
    int ID; 
    public GameObject door;
    bool moving = false;
    public bool isOpening = false;
    private PauseMenu pauseMenuScript;

    private void Start()
    {
        ObjClickScript = Cam.GetComponent<ObjClick>();
        pauseMenuScript = FindObjectOfType<PauseMenu>();
        if (pauseMenuScript != null) {
            pauseMenuScript.enabled = false;
        }
    }

    public void MoveToScene(int sceneID) 
    {
        SceneManager.LoadScene(sceneID);
    }

    void Update() 
    {
        if (ChangeTheScene) 
        {
            ChangeTheScene = false; //Keeps it from running over and over in update
            ID = ObjClickScript.SceneNumber;
            Debug.Log("Changing to Scene  " + ID);
            StartCoroutine(OpenDoor());
            moving = true;
        }
        Move(moving);
    }

    IEnumerator OpenDoor()
    {
        isOpening = true;
        door.GetComponent<Animator>().Play("DoorOpen");
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(5.0f);
        door.GetComponent<Animator>().Play("New State");
        isOpening = false;
    }

    void Move(bool Moving) //Moves towards the open door and starts scene
    {
        if (Moving) 
        {
            var step = 3.5f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Posi.transform.position, step);
            if (transform.position == Posi.transform.position) 
            {
                if (pauseMenuScript != null) {
                    pauseMenuScript.enabled = true;
                }
                moving = false;
                MoveToScene(ID);
            }
        }
    }
}

