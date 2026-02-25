using UnityEngine;

public abstract class AchievementCondition : ScriptableObject
{
    public abstract bool CheckCompletion(RunStats stats, float targetValue);
}