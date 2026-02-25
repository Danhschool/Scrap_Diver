using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleRun_Coin", menuName = "Systems/Achievements/Single Run Coin")]
public class SingleRun_Coin : AchievementCondition
{
    public override bool CheckCompletion(RunStats stats, float targetValue)
    {
        return stats.coinsCollected >= targetValue;
    }
}
