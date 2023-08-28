using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class GameManager : UnitySingleton<GameManager>
{
    public enum GameState { Start, Throwing, Thrown, ResetRat, Win}
    public GameState gameState = GameState.Start;
    public ExitGrateController grateController;
    public GameObject goalIndicator;
    private bool showIndicator = false;
    public TriggerEnterBox levelWalls;
    public PolygonCollider2D cameraConfines;
    public Transform collisionCameraPos;
    public GameObject damageText;
    public bool isPaused = false;
    public int musicProgression;
    public int throwsTaken = 0;
    public int totalDamage = 0;
    public int ratAttachCombo;
    private EventInstance levelAmbience;
    private EventInstance levelMusic;
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
        grateController = FindObjectOfType<ExitGrateController>().GetComponent<ExitGrateController>();

        StartCoroutine(StartLevel());
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0)) {
            UIManager.Instance.winUI.TogglePanel(true);
        }*/
    }

    public IEnumerator StartLevel() {
        gameState = GameState.Start;
        PlayerManager.Instance.dragText.gameObject.SetActive(false);
        levelAmbience = AudioManager.instance.CreateEventInstance(FMODEventRef.instance.SewerAmbience);
        levelAmbience.start();
        levelMusic = AudioManager.instance.CreateEventInstance(FMODEventRef.instance.LevelMusic);
        musicProgression = 0;
        RuntimeManager.StudioSystem.setParameterByName("RatProgression", 0);   
        levelMusic.start();
        AudioManager.instance.musicStarted = true;
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
        throwsTaken++;
        PlayerManager.Instance.dragText.gameObject.SetActive(true);
        gameState = GameState.Throwing;
    }

    public void WinGame()
    {
        gameState = GameState.Win;
        StartCoroutine(WinRoutine());
    }

    public IEnumerator WinRoutine()
    {
        levelAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        levelAmbience.release();
        RuntimeManager.StudioSystem.setParameterByName("RatProgression", 10);  
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

    public void ShowGoalIndicator()
    {
        //Check to see if sewer grate break indicator should spawn
        int totalRats = FindObjectsOfType<RatController>().Length;

        if (RatController.connectedRats.Count >= totalRats * grateController.requiredRatPercentage && !showIndicator)
        {
            Instantiate(goalIndicator, new Vector3(grateController.gameObject.transform.position.x - 10f, grateController.gameObject.transform.position.y, 0), Quaternion.identity);
            showIndicator = true;
        }
    }

    public void AddPoints(int val)
    {
        totalDamage += val;
        UIManager.Instance.gameUI.SetPointsText(totalDamage);
    }
}
