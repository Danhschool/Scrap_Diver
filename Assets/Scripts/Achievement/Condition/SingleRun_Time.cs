using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleRun_Time", menuName = "Systems/Achievements/Single Run Time")]
public class SingleRun_Time : AchievementCondition
{
    public override bool CheckCompletion(RunStats stats, float targetValue)
    {
        return stats.timeAlive >= targetValue;
    }
}
