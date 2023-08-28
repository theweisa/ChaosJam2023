using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{

    public TextMeshProUGUI pointsText;
    float baseFontSize;

    void Awake() {
        baseFontSize = pointsText.fontSize;
    }

    public void SetPointsText(int pts)
    {
        pointsText.text = pts.ToString();
        float newFontSize = baseFontSize + (pts / 1500f);
        LeanTween.value(pointsText.gameObject, (float val)=>{
            pointsText.fontSize = val;
        }, newFontSize*1.2f, newFontSize, 0.3f).setOnComplete(()=>{
            pointsText.fontSize = newFontSize;
        });
        //pointsText.fontSize = 36 + (pts / 1000);
    }
}
