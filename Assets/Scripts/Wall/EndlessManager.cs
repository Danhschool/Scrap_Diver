using System.Collections.Generic;
using UnityEngine;

public class EndlessManager : MonoBehaviour
{
    [Header("(Background)")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int poolSize = 5;       // Số lượng khúc tường nối đuôi nhau
    [SerializeField] private float wallHeight = 36f; // Chiều dài 1 khúc
    [SerializeField] private Vector3 spawnRotation = new Vector3(90, 0, 0);

    private List<TunnelSegment> segments = new List<TunnelSegment>();

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
            // Tính vị trí: Xếp chồng xuống dưới (âm Y)
            Vector3 spawnPos = new Vector3(0, -i * wallHeight, 0);

            GameObject obj = Instantiate(wallPrefab, spawnPos, Quaternion.Euler(spawnRotation));
            //GameObject obj = ObjectPool.instance.GetObject(wallPrefab);

            //obj.transform.position = spawnPos;
            //obj.transform.rotation = Quaternion.Euler(spawnRotation);

            TunnelSegment segment = obj.GetComponent<TunnelSegment>();
            if (segment == null) segment = obj.AddComponent<TunnelSegment>();

            // Random giao diện ngay từ đầu cho đẹp
            segment.RandomizeVisuals();

            segments.Add(segment);
        }
    }

    private void MoveSegments()
    {
        foreach (TunnelSegment segment in segments)
        {
            segment.transform.Translate(Vector3.up * GamePlayManager.instance.scrollSpeed * Time.deltaTime, Space.World);

            if (segment.transform.position.y > wallHeight + 10f)
            {
                RecycleSegment(segment);
            }
        }
    }

    private void RecycleSegment(TunnelSegment segment)
    {
        float totalLength = wallHeight * poolSize;
        segment.transform.position -= new Vector3(0, totalLength, 0);

        segment.RandomizeVisuals();
    }
}