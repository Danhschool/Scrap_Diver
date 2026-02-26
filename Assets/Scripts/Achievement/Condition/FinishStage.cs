using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FinishStage", menuName = "Systems/Achievements/FinishStage")]
public class FinishStage : AchievementCondition
{
    public override bool CheckCompletion(RunStats stats, float targetValue)
    {
        return stats.currentLevelIndex >= targetValue;
    }
}
