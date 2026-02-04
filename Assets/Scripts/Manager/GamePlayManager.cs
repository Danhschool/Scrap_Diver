using System;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance { get; private set; }

    public float scrollSpeed = 100f;

    [Header("Scale")]
    [SerializeField] private float scaleDistance = 1;
    

    [Header("Core")]
    [SerializeField] private float currentTime;
    [SerializeField] private float currentDistance;
    //[SerializeField] private float checkPoint;
    [SerializeField] private int totalCoin;
    [SerializeField] private bool isPlaying;

    [SerializeField] private int lastDistance;

    [Header("Level")]
    [SerializeField] private int indexOfLevel;
    [SerializeField] private float distanceOfLevel;


    public float CurentTime => Mathf.FloorToInt(currentTime);
    public float LastDistance => lastDistance;
    //public float CheckPoint => checkPoint;
    public int TotalCoin => totalCoin;
    public bool IsPlaying => isPlaying;
    public int IndexOfLevel => indexOfLevel;
    public float DistanceOfLevel => distanceOfLevel;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartIndex();

        Ingame_UiManager.instance.CreateMark(indexOfLevel);
    }

    private void Update()
    {
        UpdateDistance();
        UpdateTime();

        //if (Input.GetKeyDown(KeyCode.V)) UpdateCoin(1);
    }

    private void UpdateDistance()
    {
        if (!isPlaying) return;

        currentDistance += scrollSpeed * scaleDistance * Time.deltaTime;

        int currentIntDistance = (int)currentDistance;

        if (currentIntDistance > lastDistance)
        {
            lastDistance = currentIntDistance;
        }

        Ingame_UiManager.instance.UpdateDistanceUI(lastDistance);
        //Ingame_UiManager.instance.UpdateFillProgressBar(currentDistance, indexOfLevel * distanceOfLevel);
        Ingame_UiManager.instance.UpdateArrowPosition(currentIntDistance, indexOfLevel * distanceOfLevel);
    }
    private void UpdateTime()
    {
        if (!isPlaying) return;

        currentTime += Time.deltaTime;
    }

    public void UpdateCoin(int _i)
    {
        totalCoin += _i;

        Ingame_UiManager.instance.UpdateCoinUI(totalCoin);
    }

    private void StartIndex()
    {
        currentTime = 0;
        currentDistance = 0;
        //isPlaying = false;
    }

    public bool SetIsPlay(bool isPlay) => isPlaying = isPlay;

    //private float GetCheckPoint(int _point)
    //{
    //    currentDistance = _point;
    //}
}