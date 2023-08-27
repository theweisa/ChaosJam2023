using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatScript : MonoBehaviour
{
    public float downValue = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveY(gameObject, transform.position.y-downValue, 0.5f).setEaseInQuart().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
