using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjClick : MonoBehaviour
{
//Play button
    public GameObject Play;
    public GameObject Key;

//Exit Button
    public GameObject Exit;

//Settings button
    public GameObject Settings;


//Camera
    public GameObject Cam;
    private Animator camAnimator;

//Door animation
    [SerializeField] GameObject Door;
    private Animator doorAnimator;

//Cursor SFX
    [SerializeField] AudioSource HoverSound;
    bool stepOff = true; // keeps audio from being played repetedly

    public bool hoveringP = false;
    public bool hoveringS = false;
    public bool hoveringC = false;

//For Raycasting the buttons
    private int layerNumber = 8;
    private int MainMenuOptions;

    private void Start()
    {
        MainMenuOptions = 1 << layerNumber;
        doorAnimator = Door.gameObject.GetComponent<Animator>();
        camAnimator = Cam.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        PlayAnim(); //Plays Cursor animations
        if (Input.GetMouseButtonDown(0)) 
        {
            GameObject Name = GetClickedObject(out RaycastHit hit);
            if (Play == Name) // If play is clicked, start game
            {
                doorAnimator.SetBool("character_nearby", true);
                camAnimator.SetBool("SettingsOn", true); // change this to gameplay animation not settings.
                //Play.GetComponent<AudioSource>().Play();
                Debug.Log("The Scene is changing");
                //Destroy(Exit);
                //Destroy(Settings);
                //Gets rid of the NameTags too this way
                
            }
            else if (Exit == Name) //If exit is clicked quit game
            {
                Application.Quit();
            }
            else if (Settings == Name) //If Settings is clicked, play settings cam animation and open settings
            {
                doorAnimator.SetBool("character_nearby", true);
            }
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            //Debug.Log("Mouse Off");
        }
    }
    GameObject GetClickedObject(out RaycastHit hit) //Returns the name of the gameObject that has been clicked within the layer "MainMenuOptions"
    {
        //Checks if Obj is clicked on tapped on, and returns it if so
        GameObject target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 1, out hit, Mathf.Infinity, MainMenuOptions))
        {
            if (!isPointerOverUIObject()) { target = hit.collider.gameObject; }
        }
        return target;
    }
    private bool isPointerOverUIObject() //Checks to make sure it is not over an UI objects just in case
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }

    void PlayAnim() //Plays the animation sound once and bobs obj up and down when hovering over an object
    {
        GameObject Bot = GetClickedObject(out RaycastHit hit);
        if (stepOff) {
            stepOff = false;
            if (Play == Bot)
                {
                    hoveringP = true;
                    HoverSound.Play();  
                    Key.GetComponent<Bobbing>().enabled = true;       
                }
            else if (Exit == Bot)
                {
                    //.GetComponent<Bobbing>().enabled = true;
                    hoveringC = true;
                    HoverSound.Play();         
                }
            else if (Settings == Bot)
                {
                    //.GetComponent<Bobbing>().enabled = true;
                    hoveringS = true;
                    HoverSound.Play();         
                }
            else 
            {   
                Key.GetComponent<Bobbing>().enabled = false;
                //.GetComponent<Bobbing>().enabled = false;
                //.GetComponent<Bobbing>().enabled = false;
                hoveringP = false;
                hoveringS = false;
                hoveringC = false;
                HoverSound.Stop();
                stepOff = true;
            }
        }
        if (Bot == null) {
            Key.GetComponent<Bobbing>().enabled = false;
            //.GetComponent<Bobbing>().enabled = false;
            //.GetComponent<Bobbing>().enabled = false;
            hoveringP = false;
            hoveringS = false;
            hoveringC = false;
            HoverSound.Stop();
            stepOff = true;
        }
    }
}
