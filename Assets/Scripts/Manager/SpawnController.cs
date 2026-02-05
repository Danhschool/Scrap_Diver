using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [Header("Data & Settings")]
    [SerializeField] private SpawnDataSO[] spawnData;
    [SerializeField] private Obstacle_Data[] obstacleData;
    [SerializeField] private float spawnYPosition = -250f;
    //[SerializeField] private float tunnelWidth = 36f; 

    [Header("(Spawn Rate)")]
    public float timeBetweenSpawns = 2f;

    private void Start()
    {
        //StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnRoutine());
    }

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

    private void Spawn(Obstacle_Data.SpawnItem _ob)
    {
        Vector3 spawnPos = new Vector3(0, spawnYPosition, 0);

        GameObject obj = ObjectPool.instance.GetObject(_ob.prefab);

        obj.transform.position = spawnPos + _ob.position;

        Quaternion tunnelRot = Quaternion.Euler(90, 0, 0);

        obj.transform.rotation = tunnelRot * Quaternion.Euler(_ob.rotation);
    }
    private Obstacle_Data GetData()
    {
        int index = Random.Range(0, obstacleData.Length);

        return obstacleData[index];
    }

    //private IEnumerator SpawnRoutine()
    //{
    //    while (true)
    //    {
    //        SpawnDataSO data = GetData();

    //        for (int i = 0; i < data.items.Length; i++)
    //        {
    //            SpawnDataSO.SpawnItem selectedItem = data.items[i];
    //            Spawn(selectedItem);

    //            yield return new WaitForSeconds(selectedItem.timeToNextSpawn);
    //        }

    //        yield return new WaitForSeconds(timeBetweenSpawns - 0.01f);
    //    }
    //}

    //private void Spawn(SpawnDataSO.SpawnItem _item)
    //{

    //    Vector3 spawnPos = new Vector3(0, spawnYPosition, 0);

    //    Vector3 patternOffset = _item.pattern.GetCalculatedPosition(tunnelWidth);

    //    Vector3 finalPos = spawnPos + new Vector3(patternOffset.x, 0, patternOffset.z);

    //    //GameObject obj = Instantiate(item.prefab, finalPos, Quaternion.identity);
    //    GameObject obj = ObjectPool.instance.GetObject(_item.prefab);
        
    //    obj.transform.position = finalPos;

    //    Quaternion patternRot = _item.pattern.GetCalculatedRotation();

    //    Quaternion tunnelRot = Quaternion.Euler(90,0,0);

    //    obj.transform.rotation = tunnelRot * patternRot;

    //    //if (obj.GetComponent<ObstacleController>() == null)
    //    //{
    //    //    obj.AddComponent<ObstacleController>();
    //    //}
    //}

    //private SpawnDataSO GetData()
    //{
    //    int index = Random.Range(0, spawnData.Length);

    //    return spawnData[index];
    //}
}