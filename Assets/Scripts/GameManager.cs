using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    public enum GameState { Start, Throwing, Thrown, ResetRat, Win}
    public GameState gameState = GameState.Start;
    
    public TriggerEnterBox levelWalls;
    public PolygonCollider2D cameraConfines;
    public Transform collisionCameraPos;
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
        collisionCameraPos.transform.position = new Vector2(levelWalls.grate.position.x, collisionCameraPos.transform.position.y);
        CameraManager.Instance.initialCollisionCamera.transform.position = new Vector3(levelWalls.grate.position.x, collisionCameraPos.transform.position.y, CameraManager.Instance.initialCollisionCamera.transform.position.z);
        Debug.Log(cameraConfines.points.Length);
        Vector2[] newPoints = new Vector2[cameraConfines.points.Length];
        System.Array.Copy(cameraConfines.points, newPoints, cameraConfines.points.Length);
        for (int i = 0; i < newPoints.Length; i++) {
            if (newPoints[i].x > 0) {
                newPoints[i] = new Vector2(levelWalls.grate.position.x, newPoints[i].y);
                //cameraConfines.points[i].x = levelWalls.grate.position.x;
            }
        }
        cameraConfines.SetPath(0, newPoints);
        
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
        Time.timeScale = 0;
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
