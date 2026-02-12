using UnityEngine;
using System.Collections.Generic;

public enum AchievementType
{
    SingleRunCoins,
    SingleRunTime,
    TotalRobotsOwned,
    ReachLevelIndex
}

[System.Serializable]
public struct AchievementStage
{
    public float targetValue;
    public int rewardCoins;
}

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Systems/Achievement Data")]
public class AchievementData : ScriptableObject
{
    [Header("Info")]
    public string id;
    public string title;
    [TextArea] public string descriptionTemplate;
    public Sprite icon;

    [Header("Logic")]
    public AchievementType type;

    [Header("Robot")]
    public bool isRobotSpecific;
    public string robotID;

    [Header("Levels")]
    public List<AchievementStage> stages;
}