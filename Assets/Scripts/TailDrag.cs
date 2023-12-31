using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TailDrag : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public Rigidbody2D tailRb;
    public SpringJoint2D joint;
    public LineRenderer lr;
    public LayerMask layerMask;

    void Awake() {
        tailRb = tailRb != null ? tailRb : Global.FindComponent<Rigidbody2D>(gameObject);
        Camera.main.eventMask = layerMask;
    }

    private void FixedUpdate()
    {
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, PlayerManager.Instance.currentRat.GetComponent<RatController>().tailSprite.transform.position);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Throwing || PlayerManager.Instance.isHeld) return;
        Debug.Log("pickup!");
        PlayerManager.Instance.dragText.gameObject.SetActive(false);
        PlayerManager.Instance.claw.GetComponent<Animator>().SetBool("holding", false);
        Global.FadeOut(PlayerManager.Instance.claw.GetComponent<SpriteRenderer>(), 0.7f);
        LeanTween.moveY(PlayerManager.Instance.claw, PlayerManager.Instance.playerStartPosition.position.y, 1.5f).setOnComplete(()=>{
            PlayerManager.Instance.claw.SetActive(false);
        });
        mousePositionOffset = gameObject.transform.position - Global.GetMouseWorldPosition();
        PlayerManager.Instance.isHeld = true;
    }

    private void OnMouseDrag()
    {
        if (!PlayerManager.Instance.isHeld) return;
        Vector3 pos = Global.GetMouseWorldPosition() + mousePositionOffset;
        Vector3 newPos = new Vector3(
            Mathf.Min(pos.x, CameraManager.Instance.maxPoint.position.x),
            Mathf.Min(pos.y, CameraManager.Instance.maxPoint.position.y),
            pos.z
        );
        transform.position = newPos;
    }

    private void OnMouseUp()
    {
        if (!PlayerManager.Instance.isHeld) return;

        tailRb.bodyType = RigidbodyType2D.Dynamic;
        tailRb.mass = 0f;
        tailRb.angularDrag = 5f;
        PlayerManager.Instance.ThrowRat();
    }

    public void RequestPause()
    {
        GameManager.Instance.TogglePause();
    }
}
