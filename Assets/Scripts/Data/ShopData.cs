using UnityEngine;

[System.Serializable]
public class RobotData
{
    public string robotName;
    public GameObject robot;

    [Header("Description")]
    public string advantage;
    public string disadvantage;

    [Header("Price")]
    public int price;
    public bool isUnlocked;
}

[System.Serializable]
public class MapData
{
    public string mapName;
    public int price;
    public float targetMilestone;
    public GameObject pagePrefab;
}