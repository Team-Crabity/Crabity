using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject nonDestroyablePauseMenu;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button restartButton;
    public Button exitButton;
    public Button pauseButton;

    public bool isPaused = false;

    private Movement movementScript;
    private Jump jumpScript;

    public Sprite normalButtonImage;
    public Sprite pausedButtonImage;

    private AudioSlider audioSlider; // Reference to the AudioSlider script

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

        // Find the AudioSlider script in the scene
        audioSlider = FindObjectOfType<AudioSlider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButtonClick();
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ResumeGame();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RestartGame();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ReturnToMainMenu();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ExitGame();
            }

            // Check for Q and E key presses to update AudioSlider
            if (audioSlider != null)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    audioSlider.DecreaseVolume();
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    audioSlider.IncreaseVolume();
                }
            }
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

    public void PauseGame()
    {
        ChangeButtonImage(pausedButtonImage);
        TogglePauseMenu(true);
    }

    public void ResumeGame()
    {
        ChangeButtonImage(normalButtonImage);
        TogglePauseMenu(false);
    }

    public void ReturnToMainMenu()
    {
        TogglePauseMenu(false);
        SceneManager.LoadScene(0);
    }

    public void PauseButtonClick()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
        SetFocusToNull(); // Call the method to remove focus
    }

    public void RestartGame()
    {
        TogglePauseMenu(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited.");
    }

    private void SetFocusToNull()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    private void ChangeButtonImage(Sprite newImage)
    {
        Image buttonImage = pauseButton.GetComponent<Image>();

        if (buttonImage != null)
        {
            buttonImage.sprite = newImage;
        }
        else
        {
            Debug.LogError("Button component or Image component not found.");
        }
    }


}
