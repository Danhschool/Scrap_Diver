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
    [SerializeField] private GameObject p;
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
    private float coinToResume;
    public float duration = 1f;

    [SerializeField] private int lastDistance;

    [Header("Level")]
    [SerializeField] private int indexOfLevel;
    [SerializeField] private float distanceOfLevel;

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
        inGamePlayer = Instantiate(p);
        StartIndex();

        savedSpeed = gameSpeed;

        bestCoin = DataManager.BestTotalCoin;
        bestTime = DataManager.BestTime;
        bestDistance = DataManager.BestDistance;

        Ingame_UiManager.instance.UpdateProgressBar(bestDistance, distanceOfLevel * indexOfLevel, indexOfLevel);
        //Ingame_UiManager.instance.CreateMark(indexOfLevel);
        //Ingame_UiManager.instance.CreateCup(bestDistance, distanceOfLevel * indexOfLevel);

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

        Ingame_UiManager.instance.UpdateProgressCoinText(totalCoin, DataManager.ChallengeCoin);
        Ingame_UiManager.instance.UpdateProgressTimeText(CurentTime, DataManager.ChallengeTime);
    }
    public void ContinueWithCoin()
    {
        if (DataManager.TotalCoin > CalculateCoin() * 10)
        {
            isRestart = true;
            inGamePlayer = Instantiate(p);
            DataManager.TotalCoin -= CalculateCoin() * 10;
            Ingame_UiManager.instance.UpdateCoinUI(DataManager.TotalCoin);
            Ingame_UiManager.instance.SetActiveContinue_Panel(false);
            Ingame_UiManager.instance.UpdateCoinUI(totalCoin);
            StopCoroutine(coroutine);
            GameResume();

        }
        else
            return;
    }
    public int CalculateCoin()
    {
        int c = (int)currentTime / 10;

        return c;
    }
    public void GameContinueToEnd()
    {
        if (!isPlaying) return;

        Destroy(inGamePlayer);
        isPlaying = false;
        //savedSpeed = gameSpeed;
        gameSpeed = 0;
        Time.timeScale = 0;


        if (DataManager.TotalCoin > coinToResume) Ingame_UiManager.instance.UpdateContinueWithCoin(true);
        else Ingame_UiManager.instance.UpdateContinueWithCoin(false);

        Ingame_UiManager.instance.UpdateContinueWithCoin_Txt(CalculateCoin() * 10);
        Ingame_UiManager.instance.UpdateCoinUI(DataManager.TotalCoin);
        Ingame_UiManager.instance.SetActiveContinue_Panel(true);
        ClearAllObstacles();

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
    private void UpdateData()
    {
        if (DataManager.BestDistance < lastDistance)
            DataManager.BestDistance = lastDistance;
        if (DataManager.BestTime < (int)currentTime)
            DataManager.BestTime = (int)currentTime;
        if (DataManager.BestTotalCoin < totalCoin)
            DataManager.BestTotalCoin = totalCoin;

        DataManager.TotalCoin += totalCoin;
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
        if(currentDistance > bestDistance)
            Ingame_UiManager.instance.UpdateFillProgressBar(currentDistance, indexOfLevel * distanceOfLevel);
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