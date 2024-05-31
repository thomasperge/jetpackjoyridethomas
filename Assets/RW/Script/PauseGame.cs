using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas;
    public Button pauseButton;
    public Button playButton;
    public Button restartButton;

    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.SetActive(false);

        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseCanvas.SetActive(true);

        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);

        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseCanvas.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
