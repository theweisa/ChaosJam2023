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
            isAttached = true;
            joint.enabled = true;
            gameObject.layer = 7;
            GetComponent<Collider2D>().layerOverridePriority = 1;
            GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("AttachedRat");
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
        PlayerManager.Instance.currentRat = newMasterRat.gameObject;
        TailDrag tailDrag = Global.FindComponent<TailDrag>(PlayerManager.Instance.gameObject);
        tailDrag.joint = newMasterRat.joint;
        newMasterRat.joint.enabled = true;
        newMasterRat.joint.connectedBody = tailDrag.GetComponent<Rigidbody2D>();
        
    }

    public void AttachRat(RatController rat, ContactPoint2D contactPoint)
    {
        rat.transform.parent = transform;
        

        /*
        FixedJoint2D newJoint = gameObject.AddComponent<FixedJoint2D>();
        newJoint.anchor = contactPoint.point;
        newJoint.connectedBody = contactPoint.otherCollider.transform.GetComponentInParent<Rigidbody2D>();
        newJoint.enableCollision = false;
        newJoint.dampingRatio = 1;
        newJoint.breakAction = JointBreakAction2D.Ignore;
        rat.gameObject.layer = 7;
        rat.GetComponent<Collider2D>().layerOverridePriority = 1;
        rat.GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("AttachedRat");
        */

        if (tailTriggered && masterRat == this)
        {
            SetNewMasterRat(rat);
        }
        else
        {
            Destroy(rat.GetComponent<SpringJoint2D>());
            Destroy(rat.GetComponent<Rigidbody2D>());
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
