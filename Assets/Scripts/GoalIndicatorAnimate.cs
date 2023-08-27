using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalIndicatorAnimate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveX(gameObject, this.gameObject.transform.position.x - 2f, .5f).setEaseOutCubic().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
