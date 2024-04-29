using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
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
        if (PlayerManager.instance.CompanionMode == false) //single player dialogue
        {
            lines = new string[]
            {
                "Tip: advance this dialogue by using SPACE.",
                "Initiating contact...done.",
                "Greetings, CRB 1. Our records indicate this facility should be abandoned. Commence escape protocol.",
                "Preliminary scans reveal corrupt memory data: reinitializing training module... done.",
                "CRB 1: Use the W, A, S and D keys to navigate the environment.",
                "Utilize the rotation drive with LEFT SHIFT + WASD.",
                "Utilize the gravity drive with LEFT CTRL + WASD.",
            };
        }
        else //coop dialogue
        {
            lines = new string[]
            {
                "Hello! I've been transformed into a prefab!!!",
                "Tip: advance this dialogue by using either SPACE or ENTER.",
                "Initiating contact...done.",
                "Greetings, CRB 1 and 2. Our records indicate this facility should be abandoned. Commence escape protocol.",
                "Preliminary scans reveal corrupt memory data: reinitializing training module... done.",
                "CRB 1: Use the W, A, S and D keys to navigate the environment.", 
                "CRB 1: Utilize the rotation drive with LEFT SHIFT + WASD.",
                "CRB 2: Use the LEFT, RIGHT, UP, AND DOWN arrow keys to navigate the environment.",
                "CRB 2: Utilize the gravity drive with RIGHT CTRL + ARROW KEYS.",
            };
        }

        if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX) // Mac edge case
        {
            lines = new string[]
           {
                "Tip: advance this dialogue by using either SPACE or ENTER.",
                "Initiating contact...done.",
                "Greetings, CRB 1 and 2. Our records indicate this facility should be abandoned. Commence escape protocol.",
                "Preliminary scans reveal corrupt memory data: reinitializing training module... done.",
                "CRB 1: Use the W, A, S and D keys to navigate the environment.",
                "CRB 1: Utilize the rotation drive with LEFT SHIFT + WASD.",
                "CRB 2: Use the LEFT, RIGHT, UP, AND DOWN arrow keys to navigate the environment.",
                "CRB 2: Utilize the gravity drive with RIGHT OPTIONS + ARROW KEYS.",
           };
        }
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
