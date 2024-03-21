using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class hoverColorC : MonoBehaviour
{
    private TMP_Text Text; 
    public GameObject Cam;
    private ObjClick HoverScript; 
    void Start()
    {
        Text = GetComponent<TMP_Text>();
        HoverScript = Cam.GetComponent<ObjClick>();
    }

    void Update()
    {
        if (HoverScript.hoveringC)
        {
            Text.faceColor = new Color32(212, 148, 0, 255);
        }
        else
        {
            Text.faceColor = new Color32(255, 255, 255, 255);
        }
    }
}
