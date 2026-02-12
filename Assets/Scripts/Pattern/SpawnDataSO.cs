using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnData", menuName = "ScrapDivers/Spawn Data")]
public class SpawnDataSO : ScriptableObject
{
    [System.Serializable]
    public struct SpawnItem
    {
        public string name;
        public GameObject prefab;
        public SpawnPattern pattern;
        public float timeToNextSpawn;
    }

    [Header("Danh sách vật cản")]
    public SpawnItem[] items;
}