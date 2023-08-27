using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : UnitySingleton<MainMenuManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLevel(int index)
    {
        SaveManager.Instance.levelToLoad = index;
        SaveManager.Instance.currentLevel = index;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


}
