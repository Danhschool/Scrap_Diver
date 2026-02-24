//using System.Collections.Generic;
//using UnityEngine;

//public class TunnelSegment : MonoBehaviour
//{
//    [Header("Visual Settings")]
//    [SerializeField] private GameObject[] wallVariants;
//    [SerializeField] private bool randomizeRotation = true;
//    [SerializeField] private Transform visualRoot;

//    public void RandomizeVisuals()
//    {
//        Debug.Log("RandomizeVisuals called on " + gameObject.name);
//        if (wallVariants == null || wallVariants.Length == 0) return;
//        Debug.Log("Randomizing visuals for " + gameObject.name + " with " + wallVariants.Length + " variants.");

//        foreach (var variant in wallVariants) variant.SetActive(false);

//        int randomIndex = Random.Range(0, wallVariants.Length);
//        wallVariants[randomIndex].SetActive(true);

//        if (randomizeRotation && visualRoot != null)
//        {
//            float randomY = Random.Range(0, 4) * 90f;
//            visualRoot.localRotation = Quaternion.Euler(0, randomY, 0);
//        }
//    }
//}
//public class EndlessManager : MonoBehaviour
//{
//    [Header("(Background)")]
//    [SerializeField] private GameObject wallPrefab;
//    [SerializeField] private int poolSize = 5;
//    [SerializeField] private float wallHeight = 36f;
//    [SerializeField] private Vector3 spawnRotation = new Vector3(90, 0, 0);

//    private List<TunnelSegment> segments = new List<TunnelSegment>();

//    private void Start()
//    {
//        InitializePool();
//    }

//    private void Update()
//    {
//        MoveSegments();
//    }

//    private void InitializePool()
//    {
//        for (int i = 0; i < poolSize; i++)
//        {
//            Vector3 spawnPos = new Vector3(0, -i * wallHeight, 0);

//            GameObject obj = Instantiate(wallPrefab, spawnPos, Quaternion.Euler(spawnRotation));
//            //GameObject obj = ObjectPool.instance.GetObject(wallPrefab);

//            //obj.transform.position = spawnPos;
//            //obj.transform.rotation = Quaternion.Euler(spawnRotation);

//            TunnelSegment segment = obj.GetComponent<TunnelSegment>();
//            if (segment == null) segment = obj.AddComponent<TunnelSegment>();

//            segment.RandomizeVisuals();

//            segments.Add(segment);
//        }
//    }

//    private void MoveSegments()
//    {
//        foreach (TunnelSegment segment in segments)
//        {
//            segment.transform.Translate(Vector3.up * GamePlayManager.instance.GameSpeed * Time.deltaTime, Space.World);

//            if (segment.transform.position.y > wallHeight + 10f)
//            {
//                RecycleSegment(segment);
//            }
//        }
//    }

//    private void RecycleSegment(TunnelSegment segment)
//    {
//        float totalLength = wallHeight * poolSize;
//        segment.transform.position -= new Vector3(0, totalLength, 0);

//        segment.RandomizeVisuals();
//    }
//}