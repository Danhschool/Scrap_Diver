using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int bestDistance = 0;
    public int bestTime = 0;
    public int bestTotalCoin = 0;
    public int totalCoin = 0;
    public int challengeCoin = 0;
    public int challengeTime = 0;
    public int selectedPlayerIndex = 0;
    public int selectedLevelIndex = 0;
    public int levelPassed = 0;

    public int languagePref = 1;

    public bool isFirstTime = true;

    public List<string> unlockedCharacters = new List<string>();
    public int notifiedRobotIndex = -1;
    public int notifiedMapIndex = -1;

    public List<bool> unlockedLevels = new List<bool> { true, false, false, false, false };

    [System.Serializable]
    public class AchievementSave
    {
        public string id;
        public int level;
        public bool isRewardReady;
        public int unclaimedCount;
    }
    public List<AchievementSave> achievements = new List<AchievementSave>();
}