using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController instance;
    [Header("Theme")]
    //[SerializeField] private Obstacle_Data[] obstacleData;
    [SerializeField] private ObstacleTheme[] obstacleThemes;
    private int currentThemeIndex = 0;

    [Header("Data & Settings")]
    //[SerializeField] private SpawnDataSO[] spawnData;
    [SerializeField] private Obstacle_Data port;
    [SerializeField] private float spawnYPosition = -250f;
    //[SerializeField] private float tunnelWidth = 36f; 

    [Header("(Spawn Rate)")]
    public float timeBetweenSpawns = 2f;


    Coroutine spawnCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //StartCoroutine(SpawnRoutine());

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        //ChangeObstacleTheme(DataManager.SelectedLevelIndex);
        //Debug.Log($"Selected Level Index: {DataManager.SelectedLevelIndex}, Current Theme Index: {currentThemeIndex}");
        //spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public float SpawnYPosition => spawnYPosition;

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (!GamePlayManager.instance.IsPlaying)
            {
                yield return null;
                continue;
            }

            Obstacle_Data data = GetData();

            for (int i = 0; i < data.items.Length; i++)
            {
                if (!GamePlayManager.instance.IsPlaying) break;

                Obstacle_Data.SpawnItem selectedItem = data.items[i];
                Spawn(selectedItem);

                yield return new WaitForSeconds(selectedItem.timeToNextSpawn);
            }

            yield return new WaitForSeconds(timeBetweenSpawns - 0.01f);
        }
    }

    public void SpawnPort()
    {
        Spawn(port.items[0]);
    }

    private void Spawn(Obstacle_Data.SpawnItem _ob)
    {
        Vector3 spawnPos = new Vector3(0, spawnYPosition, 0);

        GameObject obj = ObjectPool.instance.GetObject(_ob.prefab);

        obj.transform.position = spawnPos + _ob.position;

        Quaternion tunnelRot = Quaternion.Euler(90, 0, 0);

        obj.transform.rotation = tunnelRot * Quaternion.Euler(_ob.rotation);
    }
    public void ChangeObstacleTheme(int newIndex)
    {
        int targetIndex = newIndex;
        Debug.Log($"Attempting to change theme to index: {targetIndex}");
        if (targetIndex >= 0 && targetIndex < obstacleThemes.Length)
        {
            currentThemeIndex = targetIndex;
        }
        if(spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
            spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private Obstacle_Data GetData()
    {
        Obstacle_Data[] currentPool = obstacleThemes[currentThemeIndex].obstacles;

        if (currentPool == null || currentPool.Length == 0) return null;

        int index = Random.Range(0, currentPool.Length);
        return currentPool[index];
    }
}