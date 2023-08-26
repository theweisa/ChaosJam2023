using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TailDrag : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public SpringJoint2D joint;
    public GameObject rat;
    public LineRenderer lr;
    private bool isHeld;

    private void FixedUpdate()
    {
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, rat.transform.position);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - Global.GetMouseWorldPosition();
        PlayerManager.Instance.ratHeld = true;
        isHeld = true;
    }

    private void OnMouseDrag()
    {
        transform.position = Global.GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (isHeld)
        {
            GameManager.Instance.ThrowRat();
            PlayerManager.Instance.ratHeld = false;
            PlayerManager.Instance.ratThrown = true;
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            rat.GetComponent<RatClamp>().ignoreClamp = true;
            this.GetComponent<Rigidbody2D>().mass = 0f; 
        }
    }
}
