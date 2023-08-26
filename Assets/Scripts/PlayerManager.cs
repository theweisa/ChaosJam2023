using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerManager : UnitySingleton<PlayerManager>
{
    public GameObject currentRat, tail;
    public PhysicsMaterial2D ratMaterial;
    public bool isHeld = false;
    public float stopVelocity = 0.2f;
    public Transform playerStartPosition, playerEndPosition;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    public IEnumerator ThrowRat() {
        isHeld = false;
        GameManager.Instance.gameState = GameManager.GameState.Thrown;
        currentRat.GetComponent<RatClamp>().ignoreClamp = true;

        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
        yield return new WaitUntil(() => currentRat.GetComponent<Rigidbody2D>().velocity.magnitude <= stopVelocity);
        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
        yield return new WaitForSeconds(3f);
        if (GameManager.Instance.gameState != GameManager.GameState.Win) {
            CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
            StartCoroutine(ResetRat());
        }
    }

    public IEnumerator ResetRat(bool start=false) {
        Rigidbody2D tailRb = tail.GetComponent<Rigidbody2D>();
        tailRb.velocity = Vector2.zero;
        tailRb.bodyType = RigidbodyType2D.Kinematic;
        currentRat.GetComponent<RatClamp>().ignoreClamp = false;
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Top, false);
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Left, false);
        SetTailPosition(playerStartPosition.position);
        LeanTween.moveY(tail, playerEndPosition.position.y, 3f);
        yield return new WaitForSeconds(3f);
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Top, true);
        if (!start) {
            GameManager.Instance.gameState = GameManager.GameState.Throwing;
        }
    }
    public void SetTailPosition(Vector2 pos, bool setRatPos=true) {
        tail.transform.position = pos;
        if (setRatPos) {
            currentRat.transform.position = tail.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
