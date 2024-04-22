using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    bool walkLeft = false;
    bool walkRight = false; 
    bool hasJumped = false;
    bool cameraRotated = false;
    bool gravShift = false;

    public Animator animator;

    private Queue <string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue <string> ();
    }

    private void Update()
    {
        //Check for left/right movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            walkLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            walkRight = true;
        }

        if(walkLeft && walkRight)
        {
            walkLeft = false;
            walkRight = false;
            DisplayNextSentence ();
        }
        //Check for jumps
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasJumped = true;
        }

        if (hasJumped)
        {
            DisplayNextSentence ();
            hasJumped = false;
        }

        //Check for camera rotation
        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W)) { 
            cameraRotated = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A))
        {
            cameraRotated = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
        {
            cameraRotated = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
        {
            cameraRotated = true;
        }

        if(cameraRotated)
        {
            DisplayNextSentence();
            cameraRotated = false;
        }
        
        //Check for gravity shift
        if(Input.GetKeyDown(KeyCode.RightControl) &&  Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gravShift = true;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravShift = true;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravShift = true;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravShift = true;
        }

        if(gravShift)
        {
            DisplayNextSentence();
            gravShift = false;
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue (sentence);
        }
        Debug.Log("Initial sentence count:" + sentences.Count.ToString());
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            Debug.Log("No more sentences\n");
            return;
        }

        string sentence = sentences.Dequeue ();
        Debug.Log(sentences.Count.ToString());
        StopAllCoroutines();
        StartCoroutine(TypeSentence (sentence));
        SetFocusToNull();
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        float waitTime = 0.03f;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
            //yield return new WaitForSeconds (waitTime); 
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    private void SetFocusToNull()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

}
