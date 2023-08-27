using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    public enum GameState { Start, Throwing, Thrown, ResetRat, Win}
    public GameState gameState = GameState.Start;
    
    public TriggerEnterBox levelWalls;
    public GameObject damageText;
    /*
        logic:
            game starts by panning to the area then panning back
            
    */
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartLevel(LevelManager level) {
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
        levelWalls.SetWallEnable(TriggerEnterBox.WallType.Left, true);
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
    }
}
