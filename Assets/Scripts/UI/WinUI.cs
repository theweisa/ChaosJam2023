using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class WinUI : MonoBehaviour
{
    public GameObject nextLevelButton;
    public TextMeshProUGUI ratText;
    public TextMeshProUGUI scoreText;


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
        yield return new WaitForSecondsRealtime(0.5f);
        ratText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.8f);
        scoreText.gameObject.SetActive(true);
        scoreText.text = GameManager.Instance.totalDamage + "";
        yield return new WaitForSecondsRealtime(0.5f);
        scoreText.text += " Damage";
        yield return new WaitForSecondsRealtime(0.8f);
        scoreText.text += " x " + RatController.connectedRats.Count;
        yield return new WaitForSecondsRealtime(0.5f);
        scoreText.text += " Rats";
        yield return new WaitForSecondsRealtime(0.5f);
        scoreText.text += " = ";
        yield return new WaitForSecondsRealtime(0.5f);
        scoreText.text += "\n\n" + (RatController.connectedRats.Count * GameManager.Instance.totalDamage);
        yield return new WaitForSecondsRealtime(0.8f);
        buttonParent.SetActive(true);
    }
}
