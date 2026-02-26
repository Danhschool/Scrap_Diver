using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockRobot", menuName = "Systems/Achievements/UnlockRobot")]
public class UnlockRobot : AchievementCondition
{
    public override bool CheckCompletion(RunStats stats, float targetValue)
    {
        return stats.robotCount > targetValue;
    }
}
