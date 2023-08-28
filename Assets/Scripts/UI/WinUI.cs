using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;


public class WinUI : MonoBehaviour
{
    public GameObject nextLevelButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI ratText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;


    public GameObject buttonParent;

    public void TogglePanel(bool state)
    {
        gameObject.SetActive(state);
        if(SceneManager.sceneCountInBuildSettings < SaveManager.Instance.currentLevel + 2)
        {
            nextLevelButton.SetActive(false);
        }
        if (state)
        {
            StartCoroutine(WinSequence());
            ratText.text = RatController.connectedRats.Count + " Rats Collected!";
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

    IEnumerator WinSequence()
    {
        Global.PopOutText(titleText, 1.6f, 0.2f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        int score = GameManager.Instance.totalDamage;
        yield return new WaitForSecondsRealtime(0.5f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(ratText);
        ratText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.8f);
        scoreText.gameObject.SetActive(true);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(scoreText);
        scoreText.text = score + "";
        yield return new WaitForSecondsRealtime(0.5f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(scoreText);
        scoreText.text += " Damage";
        yield return new WaitForSecondsRealtime(0.8f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(scoreText);
        scoreText.text += " x " + RatController.connectedRats.Count;
        yield return new WaitForSecondsRealtime(0.5f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(scoreText);
        scoreText.text += " Rats";
        yield return new WaitForSecondsRealtime(0.5f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(scoreText);
        scoreText.text += " = ";
        yield return new WaitForSecondsRealtime(0.5f);
        finalScoreText.gameObject.SetActive(true);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        Global.PopOutText(finalScoreText, 1.3f, 0.5f, LeanTweenType.easeInQuart);
        finalScoreText.text = (RatController.connectedRats.Count * score).ToString();
        yield return new WaitForSecondsRealtime(0.8f);
        RuntimeManager.PlayOneShot(FMODEventRef.instance.RatImpact, "Material", 0);
        buttonParent.SetActive(true);
    }


}
