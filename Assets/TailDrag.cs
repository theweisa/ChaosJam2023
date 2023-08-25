using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TailDrag : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public SpringJoint2D joint;
    public LineRenderer lr;
    private bool isHeld;

    private void FixedUpdate()
    {
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, joint.gameObject.transform.position);
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        isHeld = true;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (isHeld)
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
