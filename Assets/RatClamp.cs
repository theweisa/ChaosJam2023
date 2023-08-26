using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatClamp : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxAngularVelocity;
    public float unclampedVelocityModifier;
    public bool ignoreClamp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ignoreClamp && rb)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxAngularVelocity);
            if(rb.angularVelocity > maxAngularVelocity ) {
                rb.angularVelocity = maxAngularVelocity;
            }
        }
        else if (ignoreClamp && rb)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxAngularVelocity * unclampedVelocityModifier);
            if(rb.angularVelocity > maxAngularVelocity ) {
                rb.angularVelocity = maxAngularVelocity * unclampedVelocityModifier;
            }
        }
    }
}
