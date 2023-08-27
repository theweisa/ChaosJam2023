using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public GameObject nextLevelButton;

    public void TogglePanel(bool state)
    {
        gameObject.SetActive(state);
        if(SceneManager.sceneCountInBuildSettings < SaveManager.Instance.currentLevel + 2)
        {
            nextLevelButton.SetActive(false);
        }
    }

    public void NextLevel()
    {
        SaveManager.Instance.ForceLoadLevel(SaveManager.Instance.currentLevel + 1);
    }

    public void MainMenu()
    {
        SaveManager.Instance.ReturnToMainMenu();
    }
}
