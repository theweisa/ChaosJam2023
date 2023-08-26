using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTailTrigger : MonoBehaviour
{
    public RatController hostRat;
    public Rigidbody2D rb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hostRat)
        {
            hostRat.tailTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        
        if (hostRat && rb.GetContacts(contacts) == 0)
        {
            hostRat.tailTriggered = false;
        }
    }
}
