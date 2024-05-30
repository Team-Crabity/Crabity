using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }

    public GameObject pauseMenuUI;
    public GameObject nonDestroyablePauseMenu;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button restartButton;
    public Button exitButton;
    public Button pauseButton;

    public bool isPaused = false;

    private SwitchGravity gravityScript;

    public Sprite normalButtonImage;
    public Sprite pausedButtonImage;

    private AudioSlider audioSlider; // Reference to the AudioSlider script

    void Awake() {
        if (instance == null) 
        {
            instance = this;
        } else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(nonDestroyablePauseMenu);
        TogglePauseMenu(false);
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);

        // Find the Movement and Jump scripts on the player object
        gravityScript = FindObjectOfType<SwitchGravity>();

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

        if (gravityScript != null)
        {
            gravityScript.enabled = !pause;
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
        Destroy(nonDestroyablePauseMenu);
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
        if (pauseButton != null)
        {
            Image buttonImage = pauseButton.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.sprite = newImage;
            }
            else
            {
                Debug.LogError("Image component not found on the pauseButton GameObject.");
            }
        }
        else
        {
            Debug.LogError("pauseButton GameObject not assigned in the inspector.");
        }
    }

}