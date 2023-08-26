using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerManager : UnitySingleton<PlayerManager>
{
    public GameObject currentRat, tail, claw;
    public PhysicsMaterial2D ratMaterial;
    public bool isHeld = false;
    public float stopVelocity = 0.2f;
    public float stopDuration = 3f;
    public float stopTimer = 0f;
    public Transform playerStartPosition, playerEndPosition;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        stopTimer = 0f;
    }

    void Update() {
        if (GameManager.Instance.gameState == GameManager.GameState.Thrown) {
            TrackThrowVelocity();
        }
    }

    void TrackThrowVelocity() {
        if (currentRat.GetComponent<Rigidbody2D>().velocity.magnitude <= stopVelocity) {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration) {
                GameManager.Instance.gameState = GameManager.GameState.ResetRat;
                StartCoroutine(RatStopped());
            }
        }
        else {
            stopTimer = 0f;
        }
    }

    public void ThrowRat() {
        isHeld = false;
        GameManager.Instance.gameState = GameManager.GameState.Thrown;
        currentRat.GetComponent<RatClamp>().ignoreClamp = true;
        tail.GetComponent<LineRenderer>().enabled = false;
        currentRat.GetComponent<RatController>().tailSprite.enabled = true;

        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
    }

    public IEnumerator RatStopped() {
        if (GameManager.Instance.gameState != GameManager.GameState.Win) {
            CameraManager.Instance.PanToCamera(CameraManager.Instance.initialCollisionCamera);
            yield return new WaitForSeconds(2f);
            CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
            yield return ResetRat();
        }
    }

    public IEnumerator ResetRat(bool start=false) {
        stopTimer = 0f;
        tail.GetComponent<LineRenderer>().enabled = true;
        currentRat.GetComponent<RatController>().tailSprite.enabled = false;
        Rigidbody2D tailRb = tail.GetComponent<Rigidbody2D>();
        tailRb.velocity = Vector2.zero;
        tailRb.bodyType = RigidbodyType2D.Kinematic;
        currentRat.GetComponent<RatClamp>().ignoreClamp = false;
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Top, false);
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Left, false);
        SetTailPosition(playerStartPosition.position);
        claw.SetActive(true);
        claw.transform.position = playerStartPosition.position;
        LeanTween.moveY(tail, playerEndPosition.position.y, 3f);
        LeanTween.moveY(claw, playerEndPosition.position.y, 3f);
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
}
