using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : UnitySingleton<MainMenuManager>
{
    public bool gameEnabled = false;
    public bool isLoadingLevel = false;

    public CanvasGroup fadeCanvas;
    public CanvasGroup anyKeyIndicator;
    public CanvasGroup buttonsToLoad;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        LeanTween.alphaCanvas(anyKeyIndicator, 0, 2).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !gameEnabled)
        {
            gameEnabled = true;
            StartCoroutine(StartRoutine());
        }
    }

    public IEnumerator StartRoutine()
    {
        LeanTween.cancel(anyKeyIndicator.gameObject);
        yield return null;
        LeanTween.alphaCanvas(anyKeyIndicator, 0, 0.2f);
        yield return new WaitForSecondsRealtime(0.2f);
        LeanTween.moveLocalX(buttonsToLoad.gameObject, -961, 0.5f);
        LeanTween.alphaCanvas(buttonsToLoad, 1, 0.4f);
    }

    public void PlayLevel(int index)
    {
        if (isLoadingLevel)
        {
            return;
        }

        isLoadingLevel = true;
        SaveManager.Instance.levelToLoad = index;
        SaveManager.Instance.currentLevel = index;
        StartCoroutine(PlayLevelSequence());
    }

    public IEnumerator PlayLevelSequence()
    {
        fadeCanvas.blocksRaycasts = true;
        LeanTween.alphaCanvas(fadeCanvas, 1, 1);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(1);

    }


}
