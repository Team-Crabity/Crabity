using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public static DialogueTrigger instance;

    private void Awake()
    {
        instance = this;  
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        SetFocusToNull();
    }

    private void SetFocusToNull()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    private void Start()
    {
        TriggerDialogue();
    }
}
