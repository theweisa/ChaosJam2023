using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    public enum GameState { Start, Throwing, Thrown, Win}
    public GameState gameState = GameState.Start;
    /*
        logic:
            game starts by panning to the area then panning back
            
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartLevel(LevelManager level) {
        gameState = GameState.Start;
        // claw machine comes down with mouse
        // pan to the scene
        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
        // pan back
        yield return new WaitForSeconds(6f);
        CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
        yield return new WaitForSeconds(3f);
        InitThrowing();
        // let the player be draggeable and shit
    }

    public void InitThrowing() {
        gameState = GameState.Throwing;
    }
}
