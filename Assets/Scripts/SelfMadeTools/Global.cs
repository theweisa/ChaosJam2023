using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.U2D;

public static class Global {
    #nullable enable
    public static T FindComponent<T>(GameObject obj) {
        T? returnVal = obj.GetComponent<T>();
        if (returnVal != null && !returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInChildren<T>();
        if (returnVal != null && !returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInParent<T>();
        if (returnVal != null && !returnVal.Equals(null)) {
            return returnVal;
        }
        Debug.Log("ERROR: Could not Find Component");
        return returnVal;
    }
    public static Vector3 GetMouseWorldPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static float Map (this float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    public static void FadeOut(SpriteRenderer sprite, float dur) {
        Color currColor = sprite.color;
        Color fadeColor = currColor;
        fadeColor.a = 0;
        LeanTween.value(sprite.gameObject, (Color val)=>{ sprite.color=val; }, currColor, fadeColor, dur).setEase(LeanTweenType.easeInExpo);
    }
    public static void FadeIn(SpriteRenderer sprite, float dur) {
        Color currColor = sprite.color;
        Color newColor = currColor;
        newColor.a = 1;
        LeanTween.value(sprite.gameObject, (Color val)=>{ sprite.color=val; }, currColor, newColor, dur).setEase(LeanTweenType.easeInExpo);
    }

    public static void PopOutText(TMPro.TMP_Text text, float popOutAmount=1.2f, float time=0.2f, LeanTweenType ease=LeanTweenType.linear) {
        float baseFontSize = text.fontSize;
        LeanTween.value(text.gameObject, (float val)=>{
            text.fontSize = val;
        }, baseFontSize*popOutAmount, baseFontSize, time).setEase(ease).setOnComplete(()=>{
            text.fontSize = baseFontSize;
        });
    }
}