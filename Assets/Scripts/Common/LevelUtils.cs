using NUglify;
using UnityEngine;

public class LevelUtils
{
    private const string LevelStorageKey = "currentLevel";

    public static int CurrentLevel => PlayerPrefs.GetInt(LevelStorageKey);
    
    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(LevelStorageKey, level);
    }

    public static void NextLevel()
    {
        PlayerPrefs.SetInt(LevelStorageKey, CurrentLevel + 1);
    }
}