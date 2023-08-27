using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : UnitySingleton<SaveManager>
{
    public int levelToLoad = 0;
    public int currentLevel = 0;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void ForceLoadLevel(int index)
    {
        currentLevel = index;
        levelToLoad = index;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ReturnToMainMenu()
    {
        levelToLoad = 0;
        currentLevel = 0;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
