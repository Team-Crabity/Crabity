using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class M_Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private int index;
    public AudioSource source;
    public AudioClip clip;
    private string[] lines = new string[]
    {

    };

    void Start()
    {
        lines = new string[]
        {
            "Tip: advance this dialogue by using SPACE. [SPACE]",
            "Access the pause menu by pressing ESCAPE. [SPACE]",
            "Initiating contact...done. [SPACE]",
        };
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        if (!gameObject.activeSelf && Input.GetKeyDown(KeyCode.T))
        {
            gameObject.SetActive(true);
            StartDialogue();
        }
    }

    void LateUpdate()
    {
        // change companion gravity key text for Mac users
        bool isOnMac = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX;
        string companionGravityKey = isOnMac ? "RIGHT OPTION" : "RIGHT ALT or RIGHT SHIFT";
        string singlePlayerGravityKey = isOnMac ? "C" : "LEFT CTRL";

        if (PlayerManager.instance.CompanionMode == false) //single player dialogue
        {
            lines = new string[]
            {
                "Greetings, CRB 1. Our records indicate this facility should be abandoned. Commence escape protocol. [SPACE]",
                "Find and deactivate airlock seals to proceed further. [SPACE]",
                "Preliminary scans reveal corrupt memory data: reinitializing training module... done. [SPACE]",
                "CRB 1: Use the W, A, S and D keys to navigate the environment. [SPACE]",
                "Use TAB to open up the map, and TAB again to close it [SPACE]"
            };
        }
        else //coop dialogue
        {
            lines = new string[]
            {
                "Greetings, CRB 1 and 2. Our records indicate this facility should be abandoned. Commence escape protocol. [SPACE]",
                "Preliminary scans reveal corrupt memory data: reinitializing training module... done. [SPACE]",
                "CRB 1: Use the W, A, S and D keys to navigate the environment. [SPACE]",
                "CRB 2: Use the LEFT, RIGHT, UP, and DOWN arrow keys to navigate the environment. [SPACE]",
                "Use TAB to open up the map, and TAB again to close it"
            };
        }

    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
