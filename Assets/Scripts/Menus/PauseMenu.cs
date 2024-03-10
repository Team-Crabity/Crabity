using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject nonDestroyablePauseMenu;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button restartButton;
    public Button exitButton;

    public bool isPaused = false;

    private Movement movementScript;
    private Jump jumpScript;

    void Start()
    {
        DontDestroyOnLoad(nonDestroyablePauseMenu);
        TogglePauseMenu(false);
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);

        // Find the Movement and Jump scripts on the player object
        movementScript = FindObjectOfType<Movement>();
        jumpScript = FindObjectOfType<Jump>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && isPaused)
        {
            ResumeGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isPaused)
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isPaused)
        {
            ReturnToMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && isPaused)
        {
            ExitGame();
        }

    }

    void TogglePauseMenu(bool pause)
    {
        pauseMenuUI.SetActive(pause);
        Time.timeScale = pause ? 0 : 1;
        isPaused = pause;

        if (movementScript != null)
        {
            movementScript.enabled = !pause;
        }

        if (jumpScript != null)
        {
            jumpScript.enabled = !pause;
        }
    }

    void PauseGame()
    {
        TogglePauseMenu(true);
    }

    void ResumeGame()
    {
        TogglePauseMenu(false);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void RestartGame()
    {
        TogglePauseMenu(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited.");
    }

    void stopMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

    }


}
