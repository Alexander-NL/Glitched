using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign a UI panel for the pause menu (optional)
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // If the game is paused, ignore all other key inputs
        if (isPaused)
        {
            return;
        }

        // Add other input handling here if needed
        // For example:
        // if (Input.GetKeyDown(KeyCode.Space)) { ... }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(isPaused);
        }
    }
}