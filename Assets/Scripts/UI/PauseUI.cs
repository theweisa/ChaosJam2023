using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void TogglePanel(bool state)
    {
        gameObject.SetActive(state);
    }

    public void Resume()
    {
        GameManager.Instance.TogglePause(false);
    }

    public void MainMenu()
    {

    }
}
