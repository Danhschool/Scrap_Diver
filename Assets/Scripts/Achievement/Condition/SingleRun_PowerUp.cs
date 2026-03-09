using UnityEngine;

[CreateAssetMenu(fileName = "SingleRun_PowerUp", menuName = "Systems/Achievements/Single Run PowerUp")]
public class SingleRun_PowerUp : AchievementCondition
{
    public override bool CheckCompletion(RunStats stats, float targetValue)
    {
        return stats.powerUpsCollected >= targetValue;
    }
}