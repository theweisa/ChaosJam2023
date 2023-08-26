using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
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
        Debug.Log("start");
        // claw machine comes down with mouse
        // pan to the scene
        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
        // pan back
        yield return new WaitForSeconds(6f);
        CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
        yield return new WaitForSeconds(3f);
        // let the player be draggeable and shit
    }

    public IEnumerator ThrowRat() {
        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
        yield return null;
    }

    public IEnumerator PlayerTurn() {
        // allow the player to click on the mouse (show an indicator)
        // wait until the player flings the mouse
        // when they do, pan to the screen until velocity is less than a certain amount
        // pan back and redo the player turn; unless the gate breaks
        yield return null;
    }
}
