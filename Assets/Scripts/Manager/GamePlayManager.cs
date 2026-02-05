using System;
using System.Collections;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance { get; private set; }



    [Header("Scale")]
    [SerializeField] private float scaleDistance = 1;
    

    [Header("Core")]
    [SerializeField] private float currentTime;
    [SerializeField] private float currentDistance;
    //[SerializeField] private float checkPoint;
    [SerializeField] private int totalCoin;
    [SerializeField] private bool isPlaying = true;
    [SerializeField] private float gameSpeed = 100f;
    private float savedSpeed;

    [SerializeField] private int lastDistance;

    [Header("Level")]
    [SerializeField] private int indexOfLevel;
    [SerializeField] private float distanceOfLevel;


    public float CurentTime => Mathf.FloorToInt(currentTime);
    public float LastDistance => lastDistance;
    //public float CheckPoint => checkPoint;
    public int TotalCoin => totalCoin;
    public bool IsPlaying => isPlaying;
    public float GameSpeed => gameSpeed;
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

        Time.timeScale = 0;
    }

    private void Update()
    {
        UpdateDistance();
        UpdateTime();

        //if (Input.GetKeyDown(KeyCode.V)) UpdateCoin(1);
    }
    private void StartIndex()
    {
        currentTime = 0;
        currentDistance = 0;
        //isPlaying = false;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void GameResume()
    {
        isPlaying = true;
        gameSpeed = savedSpeed;
        Time.timeScale = 1;
    }

    public void GamePause()
    {
        if(!isPlaying) return;

        isPlaying = false;
        savedSpeed = gameSpeed;
        gameSpeed = 0;
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        UpdateData();

        gameSpeed = 0;

        Ingame_UiManager.instance.SetActiveContinue_Panel(true);
    }

    private void UpdateDistance()
    {
        if (!isPlaying) return;

        currentDistance += gameSpeed * scaleDistance * Time.deltaTime;

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
    private void UpdateData()
    {
        if (DataManager.BestDistance > lastDistance)
            DataManager.BestDistance = lastDistance;
        if (DataManager.BestTime > (int)currentTime)
            DataManager.BestTime = (int)currentTime;
        if (DataManager.BestTotalCoin > totalCoin)
            DataManager.BestTotalCoin = totalCoin;

        DataManager.TotalCoin += totalCoin;
    }

    public void StartCountdown() => StartCoroutine(Countdown());
    public IEnumerator Countdown(Action _onComplete = null)
    {
        int count = 3;
        while (count > 0)
        {
            Ingame_UiManager.instance.UpdateCountdown(count.ToString(), true);

            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        Ingame_UiManager.instance.UpdateCountdown("GO!", true);
        yield return new WaitForSecondsRealtime(0.5f);

        Ingame_UiManager.instance.UpdateCountdown("", false);

        Ingame_UiManager.instance.SetActiveStart_Panel(false);

        Time.timeScale = 1f;    
        isPlaying = true;

        _onComplete?.Invoke();
    }

    public bool SetIsPlay(bool isPlay) => isPlaying = isPlay;

    //private float GetCheckPoint(int _point)
    //{
    //    currentDistance = _point;
    //}
}