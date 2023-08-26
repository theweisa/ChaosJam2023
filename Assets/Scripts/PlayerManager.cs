using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : UnitySingleton<PlayerManager>
{
    public GameObject currentRat, tail;
    public bool isHeld = false;
    public float stopVelocity = 0.2f;
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
        yield return new WaitForSeconds(2f);
        if (GameManager.Instance.gameState != GameManager.GameState.Win) {
            ResetRat();
        }
    }

    public void ResetRat() {
        CameraManager.Instance.PanToCamera(CameraManager.Instance.playerCamera);
        isHeld = true;
        GameManager.Instance.gameState = GameManager.GameState.Throwing;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
