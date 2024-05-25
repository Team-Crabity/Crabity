using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> levelsOnScreen;

    private int levelsPerPage = 5;

    private int pageNum = 1;
    private int pageMax = 3;

    public GameObject levelNumPrefab;

    public RotateCutscenes rotateCutscenes;

    void Awake() {
        Debug.Log("levelsOnScreen Menu is ON");
        pageNum = 1;

        int x = 50;
        int y = 30;

        levelsOnScreen = new List<GameObject>();
        for (int i = 0; i < levelsPerPage; i += 1) {
            // Skip to next row if three levels already on row 1
            if (i != 0 && i % 3 == 0) {
                x = 90;
                y -= 100;
            }
            
            // Instantiate level selection
            Vector3 pos = new Vector3(x, y, 0);
            GameObject level = Instantiate(levelNumPrefab, transform);
            level.SetActive(true);
            level.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
            level.GetComponent<RectTransform>().anchoredPosition3D = pos;
            x += 100;
            
            // Add to list
            int levelInt = i + 1;
            levelsOnScreen.Add(level);
            levelsOnScreen[i].GetComponent<Button>().onClick.RemoveAllListeners();
            levelsOnScreen[i].GetComponent<Button>().onClick.AddListener(delegate {StartLevel(levelInt);} );
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Show next page
    public void NextPage() {
        if (pageNum < pageMax) {
            Debug.Log("Current page num: " + pageNum++);
        
            for (int i = 0; i < levelsOnScreen.Count; i += 1) {
                TMP_Text currentText = levelsOnScreen[i].GetComponentInChildren<TMP_Text>();
                int newLevelInt = int.Parse(currentText.text) + levelsPerPage;
                currentText.text = newLevelInt.ToString();
                
                levelsOnScreen[i].GetComponent<Button>().onClick.RemoveAllListeners();
                levelsOnScreen[i].GetComponent<Button>().onClick.AddListener(delegate {StartLevel(newLevelInt);} );
            }

            Debug.Log("New page num: " + pageNum);

            rotateCutscenes.RotateSettings();
        }
    }

    // Show previous page
    public void PreviousPage() {
        if (pageNum > 1) {
            Debug.Log("Current page num: " + pageNum--);
        
            for (int i = 0; i < levelsOnScreen.Count; i += 1) {
                TMP_Text currentText = levelsOnScreen[i].GetComponentInChildren<TMP_Text>();
                int newLevelInt = int.Parse(currentText.text) - levelsPerPage;
                currentText.text = newLevelInt.ToString();

                levelsOnScreen[i].GetComponent<Button>().onClick.RemoveAllListeners();
                levelsOnScreen[i].GetComponent<Button>().onClick.AddListener(delegate {StartLevel(newLevelInt);} );
            }

            Debug.Log("New page num: " + pageNum);

            rotateCutscenes.RotateCoop();
        }
    }

    // Start level
    public void StartLevel(int level) {
        Debug.Log("Starting Level " + level);
        SceneManager.LoadScene("Level " + level);
    }
}
