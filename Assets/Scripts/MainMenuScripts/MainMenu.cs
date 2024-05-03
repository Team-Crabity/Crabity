using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float delay = 2f;
    public void PlayTutorial()
    {   
        StartCoroutine(LoadGame("Tutorial 1"));
    }

    public void PlayCampaign()
    {
        StartCoroutine(LoadGame("Puzzle 1"));
    }

    public void PlayDaily()
    {
        StartCoroutine(LoadGame("Daily Puzzle"));
    }
    
    IEnumerator LoadGame(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        StartCoroutine(QuitGame());
        Debug.Log("Quit Game");
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Quit Game");
        Application.Quit();
    }
}

