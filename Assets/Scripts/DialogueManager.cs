using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    //public Animator animator;

    private Queue <string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue <string> ();
    }

   public void StartDialogue (Dialogue dialogue)
    {
        //animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue (sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue ();
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
            //yield return null;
            yield return new WaitForSeconds (waitTime); 
        }
    }

    void EndDialogue()
    {
        //animator.SetBool("isOpen", false);
    }

    private void SetFocusToNull()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

}
