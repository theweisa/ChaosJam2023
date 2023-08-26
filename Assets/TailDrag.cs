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

    void Awake() {
        tailRb = tailRb != null ? tailRb : Global.FindComponent<Rigidbody2D>(gameObject);
    }

    private void FixedUpdate()
    {
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, PlayerManager.Instance.currentRat.transform.position);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Throwing) return;
        mousePositionOffset = gameObject.transform.position - Global.GetMouseWorldPosition();
        PlayerManager.Instance.isHeld = true;
    }

    private void OnMouseDrag()
    {
        transform.position = Global.GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (PlayerManager.Instance.isHeld)
        {
            tailRb.bodyType = RigidbodyType2D.Dynamic;
            tailRb.mass = 0f;
            StartCoroutine(PlayerManager.Instance.ThrowRat());
        }
    }
}
