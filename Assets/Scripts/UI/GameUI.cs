using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{

    public TextMeshProUGUI pointsText;

    public void SetPointsText(int pts)
    {
        pointsText.text = pts.ToString();
    }
}
