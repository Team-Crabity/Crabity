using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float delay = 2f;
    public void PlayTutorial()
    {   
        StartCoroutine(LoadGame());
        SceneManager.LoadScene("Tutorial 1");
    }

    public void PlayCampaign()
    {
        StartCoroutine(LoadGame());
        SceneManager.LoadScene("Puzzle 1");
    }

    public void PlayDaily()
    {
        StartCoroutine(LoadGame());
        SceneManager.LoadScene("Daily Puzzle");
    }
    
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(delay);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}

