using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : UnitySingleton<LevelManager>
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
    public Transform levelWalls;
    void Start()
    {
        Debug.Log(GameManager.Instance);
    }
}
