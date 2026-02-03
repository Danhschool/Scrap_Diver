using System;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance { get; private set; }

    public float scrollSpeed = 100f;

    public float curentTime { get; private set; }
    public float currentDistance {  get; private set; }
    public float checkPoint {  get; private set; }
    public int totalCoin { get; private set; }
    public bool isPlaying { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartIndex();
    }

    private void StartIndex()
    {
        curentTime = 0;
        currentDistance = 0;
        isPlaying = false;
    }

    public bool SetIsPlay(bool isPlay) => isPlaying = isPlay;

    //private float GetCheckPoint(int _point)
    //{
    //    currentDistance = _point;
    //}
}