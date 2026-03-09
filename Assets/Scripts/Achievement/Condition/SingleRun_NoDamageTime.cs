using UnityEngine;

[CreateAssetMenu(fileName = "SingleRun_NoDamageTime", menuName = "Systems/Achievements/No Damage Time")]
public class SingleRun_NoDamageTime : AchievementCondition
{
    public override bool CheckCompletion(RunStats stats, float targetValue)
    {
        return stats.maxContinuousSafeTime >= targetValue;
    }
}