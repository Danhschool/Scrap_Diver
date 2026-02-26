using System;
using System.Collections;
using UnityEngine;


public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance { get; private set; }

    private int bestCoin;
    private int bestTime;
    private int bestDistance;

    [SerializeField] private Player player;
    [SerializeField] private GameObject[] p;
    private GameObject inGamePlayer;
    private bool isRestart = false;

    [Header("Scale")]
    [SerializeField] private float scaleDistance = 1;
    

    [Header("Core")]
    [SerializeField] private static float currentTime;
    [SerializeField] private static float currentDistance;
    //[SerializeField] private float checkPoint;
    [SerializeField] private static int totalCoin;
    [SerializeField] private bool isPlaying = true;
    [SerializeField] private float gameSpeed = 100f;
    public float savedSpeed;
    //private float coinToResume;
    public float duration = 1f;

    [SerializeField] private int lastDistance;

    [Header("Level")]
    [SerializeField] private int indexOfLevel;
    [SerializeField] private float distanceOfLevel;

    [Header("Portal Settings")]
    private float preSpawnDistance;
    private bool hasSpawnedPortal = false;

    private int startLevelWindow;
    private int targetLevelWindow;
    private int totalSegments;
    private float segmentWeight;

    private Coroutine coroutine;

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
        Time.timeScale = 1f;

        indexOfLevel = DataManager.SelectedLevelIndex;

        inGamePlayer = Instantiate(p[DataManager.SelectedPlayerIndex]);
        inGamePlayer.name = p[DataManager.SelectedPlayerIndex].name;

        StartIndex();

        InitializeProgressBarLogic();

        savedSpeed = gameSpeed;

        bestCoin = DataManager.BestTotalCoin;
        bestTime = DataManager.BestTime;
        bestDistance = DataManager.BestDistance;

        preSpawnDistance = scaleDistance * Mathf.Abs(SpawnController.instance.SpawnYPosition);

        Ingame_UiManager.instance.SetupDynamicProgressBar();

        //int d = (int)DataManager.GetTargetDistance(DataManager.LevelPassed + 1);

        //Ingame_UiManager.instance.UpdateProgressBar(bestDistance, d, DataManager.LevelPassed + 1);
        //Ingame_UiManager.instance.CreateMark(indexOfLevel);
        //Ingame_UiManager.instance.CreateCup(bestDistance, distanceOfLevel * indexOfLevel);

        //STime.timeScale = 0;
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

    public void ResumeGame()
    {
        isPlaying = true;
        gameSpeed = savedSpeed;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if(!isPlaying) return;

        isPlaying = false;
        savedSpeed = gameSpeed;
        gameSpeed = 0;
        Time.timeScale = 0;

        Ingame_UiManager.instance.UpdateProgressCoinText(totalCoin, DataManager.ChallengeCoin);
        Ingame_UiManager.instance.UpdateProgressTimeText(CurentTime, DataManager.ChallengeTime);
    }
    public void ContinueWithCoin()
    {
        int cost = CalculateCoin() * 10;

        if (DataManager.TrySpendCoin(cost))
        {
            isRestart = true;
            inGamePlayer = Instantiate(p[DataManager.SelectedPlayerIndex]);
            inGamePlayer.name = p[DataManager.SelectedPlayerIndex].name;

            // Cập nhật UI ngay lập tức
            Ingame_UiManager.instance.UpdateCoinUI(DataManager.TotalCoin);
            Ingame_UiManager.instance.SetActiveContinue_Panel(false);

            if (coroutine != null) StopCoroutine(coroutine);
            ResumeGame();
        }
        else
        {
            Debug.Log("Không đủ xu để hồi sinh!");
            return;
        }
    }
    public int CalculateCoin()
    {
        int c = (int)currentTime / 10;

        return c;
    }
    public void GameContinueToEnd()
    {
        if (!isPlaying) return;
        ClearAllObstacles();

        Destroy(inGamePlayer);
        isPlaying = false;
        //savedSpeed = gameSpeed;
        gameSpeed = 0;
        Time.timeScale = 0;


        if (DataManager.TotalCoin > CalculateCoin() * 10) Ingame_UiManager.instance.UpdateContinueWithCoin(true);
        else Ingame_UiManager.instance.UpdateContinueWithCoin(false);

        CheckAchivement();

        Ingame_UiManager.instance.UpdateContinueWithCoin_Txt(CalculateCoin() * 10);
        Ingame_UiManager.instance.UpdateCoinUI(DataManager.TotalCoin);
        Ingame_UiManager.instance.SetActiveContinue_Panel(true);

        if(isRestart) GameOver();

        coroutine = StartCoroutine(CountDownToEnd(()=>{
            GameOver();
            
            }));
    }

    public void GameOver()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null; 
        }
        UpdateData();

        StartCoroutine(CountUpCoroutine(currentDistance, totalCoin, currentTime));

        Ingame_UiManager.instance.SetActiveContinue_Panel(false);
        Ingame_UiManager.instance.SetActiveIngame_Panel(false);
        Ingame_UiManager.instance.pause_Btn.SetActive(false);

        Ingame_UiManager.instance.SetActiveGameOver_Panel(true);
    }

    private void CheckAchivement()
    {
        RunStats stats = new RunStats();
        stats.timeAlive = currentTime;
        stats.coinsCollected = totalCoin;
        stats.robotID = p[DataManager.SelectedPlayerIndex].name;
        stats.currentLevelIndex = indexOfLevel;

        AchievementManager.instance.CheckEndRunAchievements(stats);
    }
    private void UpdateData()
    {
        DataManager.BestDistance = lastDistance;
        DataManager.BestTime = (int)currentTime;
        DataManager.BestTotalCoin = totalCoin;

        DataManager.AddTotalCoin(totalCoin);
    }

    private void InitializeProgressBarLogic()
    {
        startLevelWindow = DataManager.SelectedLevelIndex;
        targetLevelWindow = DataManager.LevelPassed + 1;
        totalSegments = Mathf.Max(1, targetLevelWindow - startLevelWindow + 1);
        segmentWeight = 1f / totalSegments;
    }

    private void UpdateDistance()
    {
        if (!isPlaying) return;

        currentDistance += gameSpeed * scaleDistance * Time.deltaTime;

        float startDist = DataManager.GetStartDistance(indexOfLevel);
        float targetDist = DataManager.GetTargetDistance(indexOfLevel);

        float localProgress = Mathf.Clamp01((currentDistance - startDist) / (targetDist - startDist));
        int indexInWindow = indexOfLevel - startLevelWindow;
        float uiRatio = (indexInWindow * segmentWeight) + (localProgress * segmentWeight);

        int currentIntDistance = (int)currentDistance;
        if (currentIntDistance > lastDistance)
        {
            lastDistance = currentIntDistance;
            Ingame_UiManager.instance.UpdateDistanceUI(lastDistance);
        }

        if (!hasSpawnedPortal && currentDistance >= targetDist - preSpawnDistance)
        {
            hasSpawnedPortal = true;
            SpawnController.instance.SpawnPort();
        }

        if (currentDistance >= targetDist)
        {
            HandleLevelUp();
        }

        // Cập nhật UI Progress
        if (currentDistance > DataManager.BestDistance)
        {
            Ingame_UiManager.instance.UpdateFillProgressBar(uiRatio, 1f);
        }
        Ingame_UiManager.instance.UpdateArrowPosition(uiRatio, 1f);
        //Debug.Log($"Distance: {currentDistance}, UI Ratio: {uiRatio}");
    }

    private void HandleLevelUp()
    {
        indexOfLevel++;
        hasSpawnedPortal = false;

        if (indexOfLevel - 1 > DataManager.LevelPassed)
        {
            DataManager.LevelPassed = indexOfLevel - 1;

            InitializeProgressBarLogic();
            Ingame_UiManager.instance.SetupDynamicProgressBar();
        }

        PauseGame();
        ClearAllObstacles();
        TransitionManager.instance.PlayTransition(() =>
        {
            EndlessManager.instance.ChangeTheme(indexOfLevel);
            EndlessManager.instance.ChangeAllSegmentsImmediately();
            ResumeGame();
        });
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
    private void ResetData()
    {
        currentDistance = 0;
        currentTime = 0;
        totalCoin = 0;
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
    public IEnumerator CountDownToEnd(Action _onComplete = null)
    {
        float count = 5;
        while (count > 0)
        {
            Ingame_UiManager.instance.UpdateFillContinueBar(count, 5);
            count -= Time.unscaledDeltaTime;
            yield return null;
        }
        _onComplete?.Invoke();
    }
    public IEnumerator SpeedToZere()
    {
        //savedSpeed = gameSpeed;
        while(gameSpeed > 0)
        {
            gameSpeed -= 50 * Time.unscaledDeltaTime;
            yield return null;
        }
        gameSpeed = 0;
    }
    private IEnumerator CountUpCoroutine(float _dirValue, float _coinValue, float _timeValue)
    {
        int startValue = 0;
        float elapsed = 0f;
        float dir;
        float coin;
        float time;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = elapsed / duration;

            dir = (int)Mathf.Lerp(startValue, _dirValue, progress);
            coin = (int)Mathf.Lerp(startValue, _coinValue, progress);
            time = Mathf.Lerp(startValue, _timeValue, progress);

            Ingame_UiManager.instance.UpdateDistance_EndTxt((int)dir);
            Ingame_UiManager.instance.UpdateCoin_Endtxt((int)coin);
            Ingame_UiManager.instance.UpdateTime_Endtxt((int)time);

            yield return null;
        }
        dir = _dirValue;
        coin = _coinValue;
        time = _timeValue;
        Ingame_UiManager.instance.UpdateDistance_EndTxt((int)dir);
        Ingame_UiManager.instance.UpdateCoin_Endtxt((int)coin);
        Ingame_UiManager.instance.UpdateTime_Endtxt((int)time);

    }
    public bool SetIsPlay(bool isPlay) => isPlaying = isPlay;

    //private float GetCheckPoint(int _point)
    //{
    //    currentDistance = _point;
    //}
    public void ClearAllObstacles()
    {
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obj in allObstacles)
        {
            Destroy(obj);
        }
    }
}