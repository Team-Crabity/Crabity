using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    private GameOverMenu gameOverMenu;
    public float delay = 1f;

    private void Start()
    {
        gameOverMenu = FindObjectOfType<GameOverMenu>();
        if (gameOverMenu == null)
        {
            Debug.LogError("GameOverMenu script not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TriggerGameOver());
    }

    private IEnumerator TriggerGameOver()
    {
        yield return new WaitForSeconds(delay); // Wait for 1 second
        if (gameOverMenu != null)
        {
            gameOverMenu.ShowGameOverMenu();
        }
    }
}
