using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public static RatController masterRat;
    public SpringJoint2D joint;

    public bool isMaster = false;
    public bool isAttached = false;
    public bool tailTriggered;
    public bool debug;

    private void Awake()
    {
        if (isMaster)
        {
            masterRat = this;
        }
    }

    public void SetNewMasterRat(RatController newMasterRat)
    {
        if(masterRat == newMasterRat)
        {
            return;
        }

        if (masterRat != null)
        {
            masterRat.joint.enabled = false;
        }

        masterRat = newMasterRat;
        TailDrag.Instance.rat = newMasterRat.gameObject;
        TailDrag.Instance.joint = newMasterRat.joint;
        newMasterRat.joint.enabled = true;
        newMasterRat.joint.connectedBody = TailDrag.Instance.GetComponent<Rigidbody2D>();
        
    }

    public void AttachRat(RatController rat, ContactPoint2D contactPoint)
    {
        FixedJoint2D newJoint = gameObject.AddComponent<FixedJoint2D>();
        newJoint.anchor = contactPoint.point;
        newJoint.connectedBody = contactPoint.otherCollider.transform.GetComponentInParent<Rigidbody2D>();
        newJoint.enableCollision = false;
        newJoint.dampingRatio = 1;
        newJoint.breakAction = JointBreakAction2D.Ignore;

        if (tailTriggered && masterRat == this)
        {
            SetNewMasterRat(rat);
        }
    }

    private void Update()
    {
        if (debug && Input.GetKeyDown(KeyCode.U))
        {
            SetNewMasterRat(this);
        }
    }
}
