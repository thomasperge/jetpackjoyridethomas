using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas; // Canvas qui s'affiche quand le jeu est en pause
    public Button pauseButton; // Bouton de pause
    public Button playButton; // Bouton de reprise

    private bool isPaused = false;

    void Start()
    {
        // Assurez-vous que le canvas de pause est désactivé au début
        pauseCanvas.SetActive(false);

        // Assurez-vous que le bouton pause est actif et le bouton play est désactivé
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Mettre le jeu en pause
        pauseCanvas.SetActive(true); // Afficher le Canvas de pause

        // Désactiver le bouton pause et activer le bouton play
        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reprendre le jeu
        pauseCanvas.SetActive(false); // Masquer le Canvas de pause

        // Activer le bouton pause et désactiver le bouton play
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
    }
}
