using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("Canvas de pausa")]
    public GameObject desktopPauseCanvas;

    [Header("Escena del menú principal")]
    public string mainMenuSceneName = "MainMenu";

    // Agrega esta referencia
    [Header("Referencias del jugador")]
    public FirstPersonController firstPersonController;

    private bool _isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (desktopPauseCanvas != null)
            desktopPauseCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (_isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        desktopPauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Congela la cámara y el movimiento del jugador
        if (firstPersonController != null)
        {
            firstPersonController.cameraCanMove = false;
            firstPersonController.playerCanMove = false;
        }
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        desktopPauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reactiva la cámara y el movimiento
        if (firstPersonController != null)
        {
            firstPersonController.cameraCanMove = true;
            firstPersonController.playerCanMove = true;
        }
    }

    public void OnResumeButton() => Resume();

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}