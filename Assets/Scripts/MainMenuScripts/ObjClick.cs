using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjClick : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Book;
    public GameObject Gear;
    public GameObject Cam;
    [SerializeField] AudioSource HoverSound;
    private ChangeScene ChangeSceneScript;
    public int SceneNumber;
    bool stepOff = true; // keeps audio from being played repetedly
    Vector3 initPos;

    private void Start()
    {
        ChangeSceneScript = Cam.GetComponent<ChangeScene>();
    }
    void Update()
    {
        PlayAnim();
        if (Input.GetMouseButtonDown(0)) 
        {
            GameObject Name = GetClickedObject(out RaycastHit hit);
            if (Cube == Name)
            {
                SceneNumber = 1;
                ChangeSceneScript.ChangeTheScene = true;
                Cube.GetComponent<AudioSource>().Play();
                Destroy(Book.transform.parent.gameObject);
                Destroy(Gear.transform.parent.gameObject);
                //Gets rid of the NameTags too this way
                
            }
            else if (Book == Name)
            {
                SceneNumber = 2;
                ChangeSceneScript.ChangeTheScene = true;
                Book.GetComponent<AudioSource>().Play();
                Destroy(Cube.transform.parent.gameObject);
                Destroy(Gear.transform.parent.gameObject);
            }
            else if (Gear == Name)
            {
                SceneNumber = 3;
                ChangeSceneScript.ChangeTheScene = true;
                Gear.GetComponent<AudioSource>().Play();
                Destroy(Book.transform.parent.gameObject);
                Destroy(Cube.transform.parent.gameObject);
            }
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            Debug.Log("Mouse Off");
        }
    }
    GameObject GetClickedObject(out RaycastHit hit)
    {
        //Checks if Obj is clicked on tapped on, and returns it if so
        GameObject target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!isPointerOverUIObject()) { target = hit.collider.gameObject; }
        }
        return target;
    }
    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }

    void PlayAnim() 
    {
        GameObject Bot = GetClickedObject(out RaycastHit hit);
        Debug.Log(Bot);
        if (stepOff) {
            stepOff = false;
            if (Cube == Bot || Book == Bot || Gear == Bot)
                {
                    HoverSound.Play();         
                }/*
                else if (Book == Name)
                {
                    HoverSound.Play();
                }
                else if (Gear == Name)
                {
                    HoverSound.Play();
                }*/ // in case we want to play different sounds for different objects.
            else 
            {
                HoverSound.Stop();
                stepOff = true;
            }
        }
    }
}
