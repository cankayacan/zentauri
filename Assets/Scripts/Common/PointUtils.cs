
using UnityEngine;

public static class PointUtils
{
    private const string PointStorageKey = "points";

    public static int CurrentPoints => PlayerPrefs.GetInt(PointStorageKey);

    public static void SetCurrentPoints(int points)
    {
        PlayerPrefs.SetInt(PointStorageKey, points);
    }

    public static void IncrementCurrentPoints(int points = 1)
    {
        var newPoints = CurrentPoints + points;
        Debug.Log($"newPoints {newPoints}");
        PlayerPrefs.SetInt(PointStorageKey, newPoints);
    }
}