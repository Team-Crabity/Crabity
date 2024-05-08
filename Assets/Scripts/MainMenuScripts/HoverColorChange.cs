using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverColorChange : MonoBehaviour
{
    private TextMeshProUGUI Text; 
    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void ChangeColor()
    {
        Text.faceColor = new Color32(177, 156, 217, 255);
    }
    public void ChangeColorBack()
    {
        Text.faceColor = new Color32(255, 255, 255, 255);
    }

}
