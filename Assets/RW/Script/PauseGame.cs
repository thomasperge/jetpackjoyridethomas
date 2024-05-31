using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas; // Canvas qui s'affiche quand le jeu est en pause
    public Button pauseButton; // Bouton de pause
    public Button playButton; // Bouton de reprise
    public Button restartButton; // Bouton de redémarrage

    private bool isPaused = false;

    void Start()
    {
        // Assurez-vous que le canvas de pause est désactivé au début
        pauseCanvas.SetActive(false);

        // Assurez-vous que le bouton pause est actif et le bouton play est désactivé
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false); // Assurez-vous que le bouton restart est désactivé au début
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Mettre le jeu en pause
        pauseCanvas.SetActive(true); // Afficher le Canvas de pause

        // Désactiver le bouton pause et activer les boutons play et restart
        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reprendre le jeu
        pauseCanvas.SetActive(false); // Masquer le Canvas de pause

        // Activer le bouton pause et désactiver les boutons play et restart
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Assurez-vous que le temps est de nouveau en marche
        isPaused = false; // Réinitialiser l'état de pause
        pauseCanvas.SetActive(false); // Masquer le Canvas de pause

        // Recharger la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
