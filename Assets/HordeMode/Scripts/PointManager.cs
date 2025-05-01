using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    private int points;
    public HordeModeUI ui;

    public void Awake()
    {
        points = 500;
        ui.UpdatePointsText(points);
    }

    public void AddPoints(int amount)
    {
        points += amount;
        ui.UpdatePointsText(points);
    }

    public void RemovePoints(int amount)
    {
        points -= amount;
        ui.UpdatePointsText(points);
    }

    public int GetPoints()
    {
        return points;
    }

    public void DoublePoints()
    {
        points *= 2;
    }

    public void DestroyPoints()
    {
        points = 1;
    }
}
