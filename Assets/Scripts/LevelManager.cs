using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /*
    logic:
        game starts by panning to the area then panning back (angry birds)
        then it will let you pick up a rat and spin it: can spin it and it follows your cursor. cursor x position is clamped 
        after you spin and release, camera pans to the level
        after the rat reaches a low enough velocity, turn ends and it pans back to the player
        level ends once you break through the sewer grate
    */
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLevel());
    }

    public IEnumerator StartLevel() {
        // claw machine comes down with mouse
        // pan to the scene
        // pan back
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
