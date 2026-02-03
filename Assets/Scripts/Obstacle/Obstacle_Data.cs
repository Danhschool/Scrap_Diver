using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "ScrapDivers/DataSpawn")]
public class Obstacle_Data : ScriptableObject
{
    [System.Serializable]
    public struct SpawnItem
    {
        public string name;
        public GameObject prefab;
        public Vector3 position;
        public Vector3 rotation;
        public float timeToNextSpawn;
    }

    [Header("Danh sách vật cản")]
    public SpawnItem[] items;
}
