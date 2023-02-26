using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }



}
