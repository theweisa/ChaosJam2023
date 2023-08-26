using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    public enum GameState { Start, Throwing, Thrown, LevelEntered, Win}
    public GameState gameState = GameState.Start;
    public TriggerEnterBox levelWalls;
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
        levelWalls.SetLeftWallEnable(false);
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
        gameState = GameState.LevelEntered;
        levelWalls.SetLeftWallEnable(true);
    }

    public void InitThrowing() {
        gameState = GameState.Throwing;
    }
}
