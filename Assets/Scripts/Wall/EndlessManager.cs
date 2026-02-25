using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TunnelTheme
{
    [SerializeField] private string themeName;
    public GameObject[] segmentPrefabs;
}

public class EndlessManager : MonoBehaviour
{
    public static EndlessManager instance { get; private set; }

    [Header("Theme Settings")]
    [SerializeField] private TunnelTheme[] themes;
    private int currentThemeIndex = 0;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float wallHeight = 36f;
    [SerializeField] private Vector3 spawnRotation = new Vector3(90, 0, 0);
    [SerializeField] private bool randomizeRotation = true;

    private List<GameObject> activeSegments = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializePool();
    }

    private void Update()
    {
        MoveSegments();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            SpawnSegment(new Vector3(0, -i * wallHeight, 0));
        }
    }

    private void MoveSegments()
    {
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            GameObject segment = activeSegments[i];
            segment.transform.Translate(Vector3.up * GamePlayManager.instance.GameSpeed * Time.deltaTime, Space.World);

            if (segment.transform.position.y > wallHeight + 10f)
            {
                RecycleSegment(segment, i);
            }
        }
    }

    private void RecycleSegment(GameObject oldSegment, int index)
    {
        float lowestY = 0f;
        foreach (var seg in activeSegments)
        {
            if (seg.transform.position.y < lowestY)
            {
                lowestY = seg.transform.position.y;
            }
        }

        Vector3 newPos = new Vector3(0, lowestY - wallHeight, 0);

        activeSegments.RemoveAt(index);
        Destroy(oldSegment);

        SpawnSegment(newPos);
    }

    private void SpawnSegment(Vector3 position)
    {
        GameObject[] currentPrefabs = themes[currentThemeIndex].segmentPrefabs;
        GameObject prefabToSpawn = currentPrefabs[Random.Range(0, currentPrefabs.Length)];

        GameObject obj = Instantiate(prefabToSpawn, position, Quaternion.Euler(spawnRotation));

        if (obj.transform.childCount > 0)
        {
            Transform visualRoot = obj.transform.GetChild(0);

            if (randomizeRotation)
            {
                float randomY = Random.Range(0, 4) * 90f;
                visualRoot.localRotation = Quaternion.Euler(0, randomY, 0);
            }

            if (visualRoot.childCount > 0)
            {
                int randomVariant = Random.Range(0, visualRoot.childCount);
                for (int i = 0; i < visualRoot.childCount; i++)
                {
                    visualRoot.GetChild(i).gameObject.SetActive(i == randomVariant);
                }
            }
        }

        activeSegments.Add(obj);
    }

    public void ChangeTheme(int newThemeIndex)
    {
        newThemeIndex -= 1;
        if (newThemeIndex >= 0 && newThemeIndex < themes.Length)
        {
            currentThemeIndex = newThemeIndex;
        }
    }

    public void ChangeAllSegmentsImmediately()
    {
        if (activeSegments.Count == 0) return;

        float currentTopY = activeSegments[0].transform.position.y;

        foreach (var seg in activeSegments) Destroy(seg);
        activeSegments.Clear();

        for (int i = 0; i < poolSize; i++)
        {
            SpawnSegment(new Vector3(0, currentTopY - (i * wallHeight), 0));
        }
    }
}