using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : UnitySingleton<PlayerManager>
{
    public Rigidbody2D ratRb;
    public bool ratHeld = false;
    public bool ratThrown = false;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        ratRb = ratRb != null ? ratRb : Global.FindComponent<Rigidbody2D>(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
