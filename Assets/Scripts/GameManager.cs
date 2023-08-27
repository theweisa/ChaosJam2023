using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    public enum GameState { Start, Throwing, Thrown, ResetRat, Win}
    public GameState gameState = GameState.Start;
    
    public TriggerEnterBox levelWalls;
    public GameObject damageText;
    public bool isPaused = false;
    /*
        logic:
            game starts by panning to the area then panning back
            
    */
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        Time.timeScale = 1;
        
    }

    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    public IEnumerator LoadLevel()
    {
        yield return null;
        SceneManager.LoadScene(SaveManager.Instance.levelToLoad, LoadSceneMode.Additive);
        yield return null;
        levelWalls = Global.FindComponent<TriggerEnterBox>(LevelManager.Instance.levelWalls.gameObject);
        StartCoroutine(StartLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartLevel() {
        gameState = GameState.Start;
        yield return PlayerManager.Instance.ResetRat(true);
        // claw machine comes down with mouse
        // pan to the scene
        CameraManager.Instance.PanToCamera(CameraManager.Instance.initialCollisionCamera);
        // pan back
        yield return new WaitForSeconds(3.6f);
        CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
        yield return new WaitForSeconds(3f);
        InitThrowing();
        // let the player be draggeable and shit
    }

    public void OnLevelEnter() {
        if (gameState != GameState.Throwing) return;
        Debug.Log("entered!");
        //LevelManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Left, true);
    }

    public void InitThrowing() {
        gameState = GameState.Throwing;
    }

    public void WinGame()
    {
        gameState = GameState.Win;
        StartCoroutine(WinRoutine());
    }

    public IEnumerator WinRoutine()
    {
        yield return null;
        TogglePause(false);
        CameraManager.Instance.PanToCamera(CameraManager.Instance.winCamera);
        RatController.masterRat.GetComponent<Rigidbody2D>().velocity = new Vector3(40, 0, 0);
        LeanTween.value(gameObject, 0, 1, 0.8f).setOnUpdate((float val) => { Time.timeScale = Mathf.Lerp(1, 0.15f, val); }).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(0.8f);
        UIManager.Instance.winUI.TogglePanel(true);
    }

    public void TogglePause()
    {
        if(gameState == GameState.Win)
        {
            return;
        }

        isPaused = !isPaused;
        UpdatePauseState();
    }

    public void TogglePause(bool state)
    {
        if (gameState == GameState.Win)
        {
            return;
        }

        isPaused = state;
        UpdatePauseState();
    }

    private void UpdatePauseState()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            UIManager.Instance.pauseUI.TogglePanel(true);
        }
        else
        {
            Time.timeScale = 1;
            UIManager.Instance.pauseUI.TogglePanel(false);
        }
    }
}
