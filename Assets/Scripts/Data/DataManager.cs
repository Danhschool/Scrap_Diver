using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static int BestDistance
    {
        get => PlayerPrefs.GetInt(PrefConst.BEST_DISTANCE_KEY, 0);
        set
        {
            if (value <= BestDistance) return;

            PlayerPrefs.SetInt(PrefConst.BEST_DISTANCE_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static int BestTime
    {
        get => PlayerPrefs.GetInt(PrefConst.BEST_TIME_KEY, 0);

        set
        {
            if (value <= BestTime) return;

            PlayerPrefs.SetInt(PrefConst.BEST_TIME_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static int BestTotalCoin
    {
        get => PlayerPrefs.GetInt(PrefConst.BEST_TOTAL_COIN_KEY, 0);

        set
        {
            if(value <= BestTotalCoin) return;

            PlayerPrefs.SetInt(PrefConst.BEST_TOTAL_COIN_KEY, value);
            PlayerPrefs.Save();
        }
    }
    public static int TotalCoin
    {
        get => PlayerPrefs.GetInt(PrefConst.TOTAL_COIN_KEY, 0);

        set
        {
            PlayerPrefs.SetInt(PrefConst.TOTAL_COIN_KEY, value);
            PlayerPrefs.Save();
        }
    }
    public static int ChallengeCoin
    {
        get => PlayerPrefs.GetInt(PrefConst.CHALLENGE_COIN, 0);

        set
        {
            PlayerPrefs.SetInt(PrefConst.CHALLENGE_COIN, value);
            PlayerPrefs.Save();
        }
    }
    public static int ChallengeTime
    {
        get => PlayerPrefs.GetInt(PrefConst.CHALLENGE_TIME, 0);

        set
        {
            PlayerPrefs.SetInt(PrefConst.CHALLENGE_TIME, value);
            PlayerPrefs.Save();
        }
    }

    public static bool GetCharacterUnlockState(string charName)
    {
        return PlayerPrefs.GetInt(PrefConst.CHAR_UNLOCK_PREFIX + charName, 0) == 1;
    }

    public static void SetCharacterUnlockState(string charName, bool isUnlocked)
    {
        int value = isUnlocked ? 1 : 0;
        PlayerPrefs.SetInt(PrefConst.CHAR_UNLOCK_PREFIX + charName, value);
        PlayerPrefs.Save();
    }

    public static int SelectedPlayerIndex
    {
        get => PlayerPrefs.GetInt(PrefConst.SELECTED_PLAYER_KEY, 0);
        set
        {
            if (value < 0) return;
            PlayerPrefs.SetInt(PrefConst.SELECTED_PLAYER_KEY, value);
            PlayerPrefs.Save();
        }
    }
    public static int SelectedLevelIndex
    {
        get => PlayerPrefs.GetInt(PrefConst.SELECTED_LEVEL_KEY, 0);
        set
        {
            if (value < 0) return;
            PlayerPrefs.SetInt(PrefConst.SELECTED_LEVEL_KEY, value);
            PlayerPrefs.Save();
        }
    }
    public static int GetAchievementLevel(string achievementID)
    {
        return PlayerPrefs.GetInt(PrefConst.ACHIEVEMENT_LEVEL_PREFIX + achievementID, 0);
    }
    public static void SetAchievementLevel(string achievementID, int newLevel)
    {
        int currentLevel = GetAchievementLevel(achievementID);

        if (newLevel <= currentLevel) return;

        PlayerPrefs.SetInt(PrefConst.ACHIEVEMENT_LEVEL_PREFIX + achievementID, newLevel);
        PlayerPrefs.Save();
    }
    public static void ResetAchievementData(string achievementID)
    {
        PlayerPrefs.DeleteKey(PrefConst.ACHIEVEMENT_LEVEL_PREFIX + achievementID);
        PlayerPrefs.Save();
    }
}
