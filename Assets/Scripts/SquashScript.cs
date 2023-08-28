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
    public int beatsPerGroove = 3;
    Vector2 baseScale;
    Vector3 baseRotation;
    // Start is called before the first frame update
    void Awake()
    {
        baseScale = new Vector3(1,1,1);
        baseRotation = new Vector3(0,0,0);
        transform.localScale = baseScale;
        transform.localEulerAngles = baseRotation;
    }

    private void OnDisable()
    {
        transform.localScale = baseScale;
        transform.localEulerAngles = baseRotation;
    }

    public void StartGrooving() {
        int left = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        turnTimer = beatsPerGroove*(1f/(bpm/60f));
        //float squashInterval = 
        Squash();
        Rotate(left);
    }
    void Squash() {
        LeanTween.scaleY(gameObject, baseScale.y*squashCoefficient, turnTimer*0.1f).setEaseInExpo().setOnComplete(()=>{
            LeanTween.scaleY(gameObject, baseScale.y, turnTimer*0.9f).setEaseOutExpo().setOnComplete(()=>{
                Squash();
            });
        });
    }
    public IEnumerator GrooveOnBeat() {
        yield return new WaitUntil(()=>AudioManager.instance.onBeat);
        StartGrooving();
    }

    void Rotate(int left=1) {
        LeanTween.rotateLocal(gameObject, new Vector3(0f,0f,left*(turnDegree+Random.Range(-turnDegreeOffset, turnDegreeOffset))), turnTimer).setEaseOutQuart().setOnComplete(()=>{
            Rotate(-left);
        });
    }
}
