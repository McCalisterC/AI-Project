using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HordeModeUI : MonoBehaviour
{
    public TMP_Text roundText;
    public TMP_Text pointsText;

    public void UpdateRoundText(int round)
    {
        roundText.text = "Round: " + round;
    }

    public void UpdatePointsText(int points)
    {
        pointsText.text = "Points: " + points;
    }
}
