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
    public int selectedLevelIndex = 1;
    public int levelPassed = 0;

    public bool isFirstTime = true;

    public List<string> unlockedCharacters = new List<string>();

    public List<float> levelMilestones = new List<float> { 750f, 2000f, 3500f, 5500f, 8000f };
    public List<bool> unlockedLevels = new List<bool> { true, false, false, false, false, false };
    public List<int> levelPrices = new List<int> { 0, 100, 200, 200, 200, 200 };

    [System.Serializable]
    public class AchievementSave
    {
        public string id;
        public int level;
        public bool isRewardReady;
    }
    public List<AchievementSave> achievements = new List<AchievementSave>();
}