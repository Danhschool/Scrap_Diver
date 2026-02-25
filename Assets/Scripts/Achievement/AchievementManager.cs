using UnityEngine;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    [SerializeField] private List<AchievementData> allAchievements;

    public List<AchievementData> AllAchievements => allAchievements;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckEndRunAchievements(RunStats stats)
    {
        foreach (var ach in allAchievements)
        {
            int currentLv = DataManager.GetAchievementLevel(ach.id);

            if (currentLv >= ach.stages.Count) continue;
            if (ach.isRobotSpecific && ach.robotID != stats.robotID) continue;

            AchievementStage currentStage = ach.stages[currentLv];

            // Nếu chưa gán condition trong Inspector, bỏ qua để tránh lỗi NullReference
            if (ach.condition == null) continue;

            // Ủy quyền xử lý logic cho đối tượng condition
            bool isCompleted = ach.condition.CheckCompletion(stats, currentStage.targetValue);

            if (isCompleted)
            {
                UnlockLevel(ach, currentLv);
            }
        }
    }

    private void UnlockLevel(AchievementData ach, int currentLv)
    {
        DataManager.SetAchievementLevel(ach.id, currentLv + 1);
    }

    public int GetCurrentLevel(string id)
    {
        return DataManager.GetAchievementLevel(id);
    }

    #region old code
    //public static AchievementManager instance;
    //[SerializeField] private List<AchievementData> allAchievements;

    //public List<AchievementData> AllAchievements => allAchievements;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void CheckEndRunAchievements(RunStats stats)
    //{
    //    foreach (var ach in allAchievements)
    //    {
    //        int currentLv = DataManager.GetAchievementLevel(ach.id);

    //        if (currentLv >= ach.stages.Count) continue;
    //        if (ach.isRobotSpecific && ach.robotID != stats.robotID) continue;

    //        AchievementStage currentStage = ach.stages[currentLv];
    //        bool isCompleted = false;

    //        switch (ach.type)
    //        {
    //            case AchievementType.SingleRunCoins:
    //                if (stats.coinsCollected >= currentStage.targetValue) isCompleted = true;
    //                break;
    //            case AchievementType.SingleRunTime:
    //                if (stats.timeAlive >= currentStage.targetValue) isCompleted = true;
    //                break;
    //        }

    //        if (isCompleted)
    //        {
    //            UnlockLevel(ach, currentLv);
    //        }
    //    }
    //}

    //private void UnlockLevel(AchievementData ach, int currentLv)
    //{
    //    DataManager.SetAchievementLevel(ach.id, currentLv + 1);
    //}

    //public int GetCurrentLevel(string id)
    //{
    //    return DataManager.GetAchievementLevel(id);
    //}
    #endregion
}