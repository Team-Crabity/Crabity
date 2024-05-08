using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ObjClick : MonoBehaviour
{

/*//Play button
    public GameObject Play;

//Exit Button
    public GameObject Exit;
    
//Settings button
    public GameObject Settings;

//Camera
    public GameObject Cam;*/
//Cursor SFX
    [SerializeField] AudioSource HoverSound;
    //bool stepOff = true; // keeps audio from being played repetedly
    

    public bool hoveringP = false;
    /*public bool hoveringS = false;
    public bool hoveringC = false;*/

    public void PlayHover()
    {
        HoverSound.Play();
    }

    /*void Update()
    {
        //PlayAnim(); //Plays Cursor animations
        /*if (Input.GetMouseButtonDown(0)) 
        {
            GameObject Name = GetClickedObject(out RaycastHit hit);
            if (Play == Name) // If play is clicked, start game
            {
                doorAnimator.SetBool("character_nearby", true);
                camAnimator.SetBool("PlayOn", true);
                //Play.GetComponent<AudioSource>().Play();
                Debug.Log("The Scene is changing");
                //Destroy(Exit);
                if (Settings != null)
                {
                    Destroy(Settings);
                }
                //Gets rid of the NameTags too this way
                
            }
            else if (Exit == Name) //If exit is clicked quit game
            {
                Application.Quit();
            }
            else if (Settings == Name) //If Settings is clicked, play settings cam animation and open settings
            {
                doorAnimator.SetBool("character_nearby", true);
                camAnimator.SetBool("SettingsOn", true); // change this to gameplay animation not settings.
            }
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            //Debug.Log("Mouse Off");
        }
    }*/
    /*GameObject GetClickedObject(out RaycastHit hit) //Returns the name of the gameObject that has been clicked within the layer "MainMenuOptions"
    {
        //Checks if Obj is clicked on tapped on, and returns it if so
        GameObject target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 1, out hit, Mathf.Infinity, MainMenuOptions))
        {
            if (!isPointerOverUIObject()) { target = hit.collider.gameObject; }
        }
        return target;
    }*/
    /*private bool isPointerOverUIObject() //Checks to make sure it is not over an UI objects just in case
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }*/

    /*void PlayAnim() //Plays the animation sound once and bobs obj up and down when hovering over an object
    {
        //GameObject Bot = GetClickedObject(out RaycastHit hit);
        if (stepOff) {
            stepOff = false;
            if (Play == Bot)
                {
                    hoveringP = true;
                    HoverSound.Play();      
                }
            else if (Exit == Bot)
                {

                    hoveringC = true;
                    HoverSound.Play();         
                }
            else if (Settings == Bot)
                {
                    hoveringS = true;
                    HoverSound.Play();  
                    SettingsKey.GetComponent<Bobbing>().enabled = true;       
                }
            else 
            {   
                if (Settings != null) {
                    Key.GetComponent<Bobbing>().enabled = false;
                    SettingsKey.GetComponent<Bobbing>().enabled = false;
                    //.GetComponent<Bobbing>().enabled = false;
                    hoveringP = false;
                    hoveringS = false;
                    hoveringC = false;
                    HoverSound.Stop();
                    stepOff = true;
                }
            }
        }
        if (Bot == null) {
            if (Settings != null) {
                Key.GetComponent<Bobbing>().enabled = false;
                SettingsKey.GetComponent<Bobbing>().enabled = false;
                //.GetComponent<Bobbing>().enabled = false;
                hoveringP = false;
                hoveringS = false;
                hoveringC = false;
                HoverSound.Stop();
                stepOff = true;
            }
        }
    }

    public void OpenPlayDoor()
    {
        PlayDoorAnimator.SetBool("character_nearby", true);
    }

    public void LoadScene() 
    {
        SceneManager.LoadScene(1);
    }*/
}
