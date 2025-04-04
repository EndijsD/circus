using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public  GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject victoryMenu;
    public GameObject leaderboardUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        victoryMenu.SetActive(false);
        leaderboardUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !victoryMenu.activeSelf)
        {
            if (GameIsPaused)
            {
                if (settingsMenuUI.activeSelf)
                {
                    settingsMenuUI.SetActive(false);
                    pauseMenuUI.SetActive(true);
                }else if (leaderboardUI.activeSelf)
                {
                    leaderboardUI.SetActive(false);
                    pauseMenuUI.SetActive(true);
                } else
                    Resume();
            }
            else
            {
                Pause();
            }
        }
    }

     public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void OpenLeaderboard()
    {
        pauseMenuUI.SetActive(false);
        leaderboardUI.SetActive(true);
    }
}
