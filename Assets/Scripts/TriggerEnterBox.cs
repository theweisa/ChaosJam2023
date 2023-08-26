using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterBox : MonoBehaviour
{
    public enum WallType { Top, Left };
    public Collider2D leftWall, topWall;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag != "Player") return;
        GameManager.Instance.OnLevelEnter();
    }
    public void SetWallEnable(WallType type, bool enabled) {
        Collider2D wall = null;
        switch(type) {
            case WallType.Top:
                wall = topWall;
                break;
            case WallType.Left:
                wall = leftWall;
                break;
        }
        if (!wall) return;
        wall.enabled = enabled;
    }
}
