using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterBox : MonoBehaviour
{
    public Collider2D leftWall;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag != "Player") return;
        GameManager.Instance.OnLevelEnter();
    }
    public void SetLeftWallEnable(bool enabled) {
        leftWall.enabled = enabled;
    }
}
