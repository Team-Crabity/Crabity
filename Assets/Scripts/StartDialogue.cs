using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        DialogueTrigger.instance.TriggerDialogue();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
