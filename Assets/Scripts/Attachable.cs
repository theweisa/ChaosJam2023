using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<RatController>())
        {
            RatController collidedRat = collision.transform.GetComponent<RatController>();



        }
    }
}
