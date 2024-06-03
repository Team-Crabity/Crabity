using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public static GameOverMenu instance { get; private set; }

    public GameObject gameOverMenuUI;
    public Button restartButton;
    public Button mainMenuButton;
    public Button exitButton;

    private TimeTracker timeTracker;
    private bool menuActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverMenuUI.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        exitButton.onClick.AddListener(ExitGame);

        // Find the TimeTracker script in the scene
        timeTracker = FindObjectOfType<TimeTracker>();
    }

    private void Update()
    {
        if (menuActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ReturnToMainMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ExitGame();
            }
        }
    }

    public void ShowGameOverMenu()
    {
        if (timeTracker != null)
        {
            timeTracker.StopTracking();
        }

        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        menuActive = true;
    }

    private void HideGameOverMenu()
    {
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        menuActive = false;
    }

    public void RestartGame()
    {
        HideGameOverMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        HideGameOverMenu();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited.");
    }
}
