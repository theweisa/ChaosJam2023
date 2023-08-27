using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public void TogglePanel(bool state)
    {
        gameObject.SetActive(state);
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
