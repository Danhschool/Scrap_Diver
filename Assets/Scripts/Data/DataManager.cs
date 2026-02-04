using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static int BestDistance
    {
        get => PlayerPrefs.GetInt(PrefConst.BEST_DISTANCE_KEY, 0);
        set
        {
            if (value <= BestDistance) return;

            PlayerPrefs.SetInt(PrefConst.BEST_DISTANCE_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static int BestTime
    {
        get => PlayerPrefs.GetInt(PrefConst.BEST_TIME_KEY, 0);

        set
        {
            if (value <= BestTime) return;

            PlayerPrefs.SetInt(PrefConst.BEST_TIME_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static int BestTotalCoin
    {
        get => PlayerPrefs.GetInt(PrefConst.BEST_TOTAL_COIN_KEY, 0);

        set
        {
            if(value <= BestTotalCoin) return;

            PlayerPrefs.SetInt(PrefConst.BEST_TOTAL_COIN_KEY, value);
            PlayerPrefs.Save();
        }
    }
    public static int TotalCoin
    {
        get => PlayerPrefs.GetInt(PrefConst.TOTAL_COIN_KEY, 0);

        set
        {
            PlayerPrefs.SetInt(PrefConst.TOTAL_COIN_KEY, value);
            PlayerPrefs.Save();
        }
    }
}
