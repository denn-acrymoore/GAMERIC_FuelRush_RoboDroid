using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;

    public void OnPauseButtonPressed()
    {
        CustomGridManager.isMenuOpened = true;
        pauseMenuPanel.SetActive(true);
    }

    public void OnResumeButtonPressed()
    {
        CustomGridManager.isMenuOpened = false;
        pauseMenuPanel.SetActive(false);
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
