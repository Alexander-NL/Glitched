using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI components

public class SceneLoader : MonoBehaviour
{
    [Header("Button Settings")]
    public Image buttonImage;

    [Header("Scene Load Delay")]
    public float delayBeforeLoad = 1f;

    public void LoadSceneWithDelay(string sceneName)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delayBeforeLoad));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    // Method to exit the application
    public void ExitApp()
    {
        Application.Quit();
    }

    public void ChangeButtonImage(Sprite newSprite)
    {
        if (buttonImage != null && newSprite != null)
        {
            buttonImage.sprite = newSprite; 
        }
        else
        {
            Debug.LogWarning("Button Image or New Sprite is not assigned!");
        }
    }
}