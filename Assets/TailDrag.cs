using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailDrag : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public SpringJoint2D spring;
    public LineRenderer lr;

    private void Update()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, spring.connectedBody.position);
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
}
