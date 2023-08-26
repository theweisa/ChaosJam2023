using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float angle = 20f;
    public float angleOffset = 5f;
    public float textDist = 0.7f;
    public float textDur = 0.7f;
    public float critDamageThreshold = 100f;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Init(float dmg) {
        if (dmg > critDamageThreshold) {
            text.fontSize *= 1.25f;
            text.color = Color.red;
        }
        text.text = dmg.ToString("F0");
        float newAngle = (angle+UnityEngine.Random.Range(-angleOffset, angleOffset))*Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * textDist;
        LeanTween.move(gameObject, (Vector2)transform.position + dir, textDur).setEase(LeanTweenType.easeOutQuart).setOnComplete(()=>{
            Destroy(gameObject);
        });

        Color currColor = text.color;
        Color fadeColor = currColor;
        fadeColor.a = 0;
        LeanTween.value(gameObject, (Color val)=>{ text.color=val; }, currColor, fadeColor, textDur).setEase(LeanTweenType.easeInExpo);
    }
}
