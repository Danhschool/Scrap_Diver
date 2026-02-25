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

    public List<string> unlockedCharacters = new List<string>();

    [System.Serializable]
    public class AchievementSave
    {
        public string id;
        public int level;
        public bool isRewardReady;
    }
    public List<AchievementSave> achievements = new List<AchievementSave>();
}