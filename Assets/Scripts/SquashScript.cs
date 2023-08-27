using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashScript : MonoBehaviour
{
    public float bpm = 170f;
    float turnTimer;
    public float turnDegree = 7f;
    public float turnDegreeOffset = 2f;
    public float squashCoefficient = 0.8f;
    Vector2 baseScale;
    // Start is called before the first frame update
    void Start()
    {
        int left = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        turnTimer = 1f/(bpm/60f);
        baseScale = transform.localScale;
        //float squashInterval = 
        LeanTween.scaleY(gameObject, baseScale.y*squashCoefficient, turnTimer*0.5f).setLoopPingPong().setEaseInExpo();
        Rotate(left);
    }

    void Rotate(int left=1) {
        LeanTween.rotateLocal(gameObject, new Vector3(0f,0f,left*(turnDegree+Random.Range(-turnDegreeOffset, turnDegreeOffset))), turnTimer).setEaseOutQuart().setOnComplete(()=>{
            Rotate(-left);
        });
    }
}
