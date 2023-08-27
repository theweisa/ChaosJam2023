using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public static RatController masterRat;
    public static List<RatController> connectedRats = new List<RatController>();
    public SpringJoint2D joint;
    public SpriteRenderer tailSprite;
    public GameObject stickyParticlePrefab;
    public Vector3 preVelocity = Vector3.zero;

    public bool isMaster = false;
    public bool isAttached = false;
    public bool tailTriggered;
    public bool debug;

    private void Start()
    {
        tailSprite.enabled = false;
        if (isMaster)
        {
            masterRat = this;
            isAttached = true;
            joint.enabled = true;
            gameObject.layer = 7;
            joint.connectedBody = PlayerManager.Instance.tail.GetComponent<Rigidbody2D>();
            GetComponent<Collider2D>().layerOverridePriority = 1;
            GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("AttachedRat");
            connectedRats.Add(this);
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

            if (newMasterRat.joint == null && newMasterRat.GetComponent<SpringJoint2D>() == null)
            {
                ComponentHelper.CopyComponent<SpringJoint2D>(masterRat.joint, newMasterRat.gameObject);
                newMasterRat.joint = newMasterRat.GetComponent<SpringJoint2D>();
                Destroy(masterRat.joint);
            }

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
        rat.GetComponent<Rigidbody2D>().sharedMaterial = PlayerManager.Instance.ratMaterial;

        var sticky = Instantiate(stickyParticlePrefab, contactPoint.point, Quaternion.identity);
        sticky.transform.parent = transform;
        Global.FindComponent<Animator>(rat.gameObject).SetBool("hanging", true);
        rat.tailSprite.enabled = true;
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

        connectedRats.Add(rat);

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

    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>())
        {
            preVelocity = GetComponent<Rigidbody2D>().velocity;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        
    }
}
