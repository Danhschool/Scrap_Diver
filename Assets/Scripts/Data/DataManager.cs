using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private static IDataService dataService;
    private static GameData currentData;

    static DataManager()
    {
        dataService = new JsonDataService();
        currentData = dataService.Load();
    }

    private static void SaveToDisk()
    {
        dataService.Save(currentData);
    }

    public static int BestDistance
    {
        get => currentData.bestDistance;
        set
        {
            if (value <= currentData.bestDistance) return;
            currentData.bestDistance = value;
            SaveToDisk();
        }
    }

    public static int BestTime
    {
        get => currentData.bestTime;
        set
        {
            if (value <= currentData.bestTime) return;
            currentData.bestTime = value;
            SaveToDisk();
        }
    }

    public static int BestTotalCoin
    {
        get => currentData.bestTotalCoin;
        set
        {
            if (value <= currentData.bestTotalCoin) return;
            currentData.bestTotalCoin = value;
            SaveToDisk();
        }
    }

    public static int TotalCoin
    {
        get => currentData.totalCoin;
        set
        {
            currentData.totalCoin = value;
            SaveToDisk();
        }
    }

    public static int ChallengeCoin
    {
        get => currentData.challengeCoin;
        set
        {
            currentData.challengeCoin = value;
            SaveToDisk();
        }
    }

    public static int ChallengeTime
    {
        get => currentData.challengeTime;
        set
        {
            currentData.challengeTime = value;
            SaveToDisk();
        }
    }

    public static void AddTotalCoin(int amount)
    {
        currentData.totalCoin += amount;
        SaveToDisk();
    }

    public static int SelectedPlayerIndex
    {
        get => currentData.selectedPlayerIndex;
        set
        {
            if (value < 0) return;
            currentData.selectedPlayerIndex = value;
            SaveToDisk();
        }
    }

    public static int SelectedLevelIndex
    {
        get => currentData.selectedLevelIndex;
        set
        {
            if (value < 0) return;
            currentData.selectedLevelIndex = value;
            SaveToDisk();
        }
    }

    public static bool GetCharacterUnlockState(string charName)
    {
        return currentData.unlockedCharacters.Contains(charName);
    }

    public static void SetCharacterUnlockState(string charName, bool isUnlocked)
    {
        bool hasChar = currentData.unlockedCharacters.Contains(charName);

        if (isUnlocked && !hasChar)
        {
            currentData.unlockedCharacters.Add(charName);
            SaveToDisk();
        }
        else if (!isUnlocked && hasChar)
        {
            currentData.unlockedCharacters.Remove(charName);
            SaveToDisk();
        }
    }

    private static GameData.AchievementSave GetOrCreateAchievement(string id)
    {
        var ach = currentData.achievements.Find(a => a.id == id);
        if (ach == null)
        {
            ach = new GameData.AchievementSave { id = id, level = 0, isRewardReady = false };
            currentData.achievements.Add(ach);
        }
        return ach;
    }

    public static int GetAchievementLevel(string achievementID)
    {
        return GetOrCreateAchievement(achievementID).level;
    }

    public static void SetAchievementLevel(string achievementID, int newLevel)
    {
        var ach = GetOrCreateAchievement(achievementID);
        if (newLevel <= ach.level) return;

        ach.level = newLevel;
        SaveToDisk();
    }

    public static void ResetAchievementData(string achievementID)
    {
        int removedCount = currentData.achievements.RemoveAll(a => a.id == achievementID);
        if (removedCount > 0) SaveToDisk();
    }

    public static bool IsRewardReady(string id)
    {
        return GetOrCreateAchievement(id).isRewardReady;
    }

    public static void SetRewardReady(string id, bool isReady)
    {
        var ach = GetOrCreateAchievement(id);
        ach.isRewardReady = isReady;
        SaveToDisk();
    }

    public static bool TrySpendCoin(int amount)
    {
        if (currentData.totalCoin >= amount)
        {
            currentData.totalCoin -= amount;
            SaveToDisk();
            return true;
        }
        return false;
    }
    #region code old
    //public static int BestDistance
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.BEST_DISTANCE_KEY, 0);
    //    set
    //    {
    //        if (value <= BestDistance) return;

    //        PlayerPrefs.SetInt(PrefConst.BEST_DISTANCE_KEY, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int BestTime
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.BEST_TIME_KEY, 0);

    //    set
    //    {
    //        if (value <= BestTime) return;

    //        PlayerPrefs.SetInt(PrefConst.BEST_TIME_KEY, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int BestTotalCoin
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.BEST_TOTAL_COIN_KEY, 0);

    //    set
    //    {
    //        if(value <= BestTotalCoin) return;

    //        PlayerPrefs.SetInt(PrefConst.BEST_TOTAL_COIN_KEY, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int TotalCoin
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.TOTAL_COIN_KEY, 0);

    //    set
    //    {
    //        PlayerPrefs.SetInt(PrefConst.TOTAL_COIN_KEY, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int ChallengeCoin
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.CHALLENGE_COIN, 0);

    //    set
    //    {
    //        PlayerPrefs.SetInt(PrefConst.CHALLENGE_COIN, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int ChallengeTime
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.CHALLENGE_TIME, 0);

    //    set
    //    {
    //        PlayerPrefs.SetInt(PrefConst.CHALLENGE_TIME, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static bool GetCharacterUnlockState(string charName)
    //{
    //    return PlayerPrefs.GetInt(PrefConst.CHAR_UNLOCK_PREFIX + charName, 0) == 1;
    //}
    //public static void SetCharacterUnlockState(string charName, bool isUnlocked)
    //{
    //    int value = isUnlocked ? 1 : 0;
    //    PlayerPrefs.SetInt(PrefConst.CHAR_UNLOCK_PREFIX + charName, value);
    //    PlayerPrefs.Save();
    //}
    //public static int SelectedPlayerIndex
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.SELECTED_PLAYER_KEY, 0);
    //    set
    //    {
    //        if (value < 0) return;
    //        PlayerPrefs.SetInt(PrefConst.SELECTED_PLAYER_KEY, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int SelectedLevelIndex
    //{
    //    get => PlayerPrefs.GetInt(PrefConst.SELECTED_LEVEL_KEY, 1);
    //    set
    //    {
    //        if (value < 0) return;
    //        PlayerPrefs.SetInt(PrefConst.SELECTED_LEVEL_KEY, value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public static int GetAchievementLevel(string achievementID)
    //{
    //    return PlayerPrefs.GetInt(PrefConst.ACHIEVEMENT_LEVEL_PREFIX + achievementID, 0);
    //}
    //public static void SetAchievementLevel(string achievementID, int newLevel)
    //{
    //    int currentLevel = GetAchievementLevel(achievementID);

    //    if (newLevel <= currentLevel) return;

    //    PlayerPrefs.SetInt(PrefConst.ACHIEVEMENT_LEVEL_PREFIX + achievementID, newLevel);
    //    PlayerPrefs.Save();
    //}
    //public static void ResetAchievementData(string achievementID)
    //{
    //    PlayerPrefs.DeleteKey(PrefConst.ACHIEVEMENT_LEVEL_PREFIX + achievementID);
    //    PlayerPrefs.Save();
    //}
    //public static bool IsRewardReady(string id)
    //{
    //    return PlayerPrefs.GetInt(PrefConst.ACH_READY_PREFIX + id, 0) == 1;
    //}
    //public static void SetRewardReady(string id, bool isReady)
    //{
    //    PlayerPrefs.SetInt(PrefConst.ACH_READY_PREFIX + id, isReady ? 1 : 0);
    //    PlayerPrefs.Save();
    //}
    #endregion
}
