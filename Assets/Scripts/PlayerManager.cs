using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerManager : UnitySingleton<PlayerManager>
{
    public GameObject currentRat, tail, claw;
    public PhysicsMaterial2D ratMaterial;
    public bool isHeld = false;
    public float stopVelocity = 0.2f;
    public float stopDuration = 3f;
    public float stopTimer = 0f;
    public float lifespan = 10f;
    private float lifespanTimer = 0f;
    private EventInstance ratFlying;
    private EventInstance ratSpinning;
    public int ratAttachCombo;
    public Transform playerStartPosition, playerEndPosition;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        stopTimer = 0f;
        lifespanTimer = 0f;
        if (RatController.connectedRats != null)
        {
            RatController.connectedRats.Clear();
            if (RatController.masterRat)
            {
                RatController.connectedRats.Add(RatController.masterRat);
            }

        }
    }

    void Update() {
        if (GameManager.Instance.gameState == GameManager.GameState.Thrown) {
            TrackThrowVelocity();
        }
    }

    void TrackThrowVelocity() {
        lifespanTimer += Time.deltaTime;
        if (lifespanTimer > lifespan) {
            StartCoroutine(RatStopped());
            return;
        }
        if (currentRat.GetComponent<Rigidbody2D>().velocity.magnitude <= stopVelocity) {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration) {
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
        
        ratFlying = AudioManager.instance.CreateEventInstance(FMODEventRef.instance.RatFlying);
        ratFlying.setParameterByName("RatFlightTime", 0);
        currentRat.GetComponent<RatController>().ratFlightTime = 0;
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventRef.instance.RatRelease);
        ratSpinning.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ratSpinning.release();
        ratFlying.start();

        CameraManager.Instance.PanToCamera(CameraManager.Instance.collisionCamera);
    }

    public IEnumerator RatStopped() {
        if (GameManager.Instance.gameState == GameManager.GameState.Thrown) {
            GameManager.Instance.gameState = GameManager.GameState.ResetRat;
            CameraManager.Instance.PanToCamera(CameraManager.Instance.initialCollisionCamera);
            GameManager.Instance.ShowGoalIndicator();
            //yield return new WaitForSeconds(2f);
            yield return PickUpRat();
            CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
            yield return new WaitForSeconds(1f);
            yield return ResetRat();
        }
    }

    public IEnumerator PickUpRat() {
        ratFlying.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ratFlying.release();
        Rigidbody2D tailRb = tail.GetComponent<Rigidbody2D>();
        Vector2 tailPos = currentRat.GetComponent<RatController>().tailSprite.transform.position;
        claw.SetActive(true);
        Global.FadeIn(claw.GetComponent<SpriteRenderer>(), 0.3f);
        claw.GetComponent<Animator>().SetBool("holding", false);
        claw.transform.position = new Vector2(tailPos.x, playerStartPosition.position.y);
        LeanTween.moveY(claw, currentRat.GetComponent<RatController>().tailSprite.transform.position.y, 2f);
        yield return new WaitForSeconds(3f);
        claw.GetComponent<Animator>().SetBool("holding", true);

        foreach (Collider2D coll in currentRat.GetComponents<Collider2D>()) {
            coll.enabled = false;
        }
        foreach (Collider2D coll in currentRat.GetComponentsInChildren<Collider2D>()) {
            coll.enabled = false;
        }
        foreach (SpriteRenderer sprite in currentRat.GetComponents<SpriteRenderer>()) {
            Global.FadeOut(sprite, 1f);
        }
        foreach (SpriteRenderer sprite in currentRat.GetComponentsInChildren<SpriteRenderer>()) {
            Global.FadeOut(sprite, 1f);
        }
        tailRb.velocity = Vector2.zero;
        tailRb.bodyType = RigidbodyType2D.Kinematic;
        currentRat.GetComponent<RatClamp>().ignoreClamp = false;
        SetTailPosition(tailPos, false);
        Global.FadeOut(claw.GetComponent<SpriteRenderer>(), 1f);
        LeanTween.moveY(tail, tail.transform.position.y + 10f, 1f);
        LeanTween.moveY(claw, tail.transform.position.y + 10f, 1f);
    }

    public IEnumerator ResetRat(bool start=false) {
        ratSpinning = AudioManager.instance.CreateEventInstance(FMODEventRef.instance.RatSwinging);
        ratSpinning.start();
        ratAttachCombo = 0;
        stopTimer = 0f;
        lifespanTimer = 0f;
        tail.GetComponent<LineRenderer>().enabled = true;
        currentRat.GetComponent<RatController>().tailSprite.enabled = false;
        Global.FindComponent<Animator>(currentRat).SetBool("hanging", true);
        foreach (Collider2D coll in currentRat.GetComponents<Collider2D>()) {
            coll.enabled = true;
        }
        foreach (Collider2D coll in currentRat.GetComponentsInChildren<Collider2D>()) {
            coll.enabled = true;
        }
        foreach (SpriteRenderer sprite in currentRat.GetComponents<SpriteRenderer>()) {
            Global.FadeIn(sprite, 0.5f);
        }
        foreach (SpriteRenderer sprite in currentRat.GetComponentsInChildren<SpriteRenderer>()) {
            Global.FadeIn(sprite, 0.5f);
        }

        Rigidbody2D tailRb = tail.GetComponent<Rigidbody2D>();
        tailRb.velocity = Vector2.zero;
        tailRb.bodyType = RigidbodyType2D.Kinematic;
        currentRat.GetComponent<RatClamp>().ignoreClamp = false;
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Top, false);
        GameManager.Instance.levelWalls.SetWallEnable(TriggerEnterBox.WallType.Left, false);
        SetTailPosition(playerStartPosition.position);
        claw.SetActive(true);
        Global.FadeIn(claw.GetComponent<SpriteRenderer>(), 0.5f);
        claw.GetComponent<Animator>().SetBool("holding", true);
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
