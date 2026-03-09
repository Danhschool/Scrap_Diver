using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    [Header("Robot Specific Config")]
    [SerializeField] private RobotPassiveData myPassive;

    [Header("Tracking Data")]
    public int powerUpsCollected;
    public float maxContinuousSafeTime;

    private float _currentSafeTime;
    private bool _isInitialized = false;

    private void Start()
    {
        ApplyMyPassives();
    }

    private void ApplyMyPassives()
    {
        if (myPassive != null && myPassive.passiveModules != null)
        {
            foreach (var module in myPassive.passiveModules)
            {
                if (module != null)
                {
                    module.ApplyPassive(this.gameObject, GamePlayManager.instance);
                }
            }
        }
        _isInitialized = true;
    }

    private void Update()
    {
        if (!GamePlayManager.instance.IsPlaying || !_isInitialized) return;

        _currentSafeTime += Time.deltaTime;
        if (_currentSafeTime > maxContinuousSafeTime)
        {
            maxContinuousSafeTime = _currentSafeTime;

            AchievementManager.instance.CheckAchievementsByType<SingleRun_NoDamageTime>(GetCurrentStats());
        }
    }

    public void AddPowerUp() { 
        powerUpsCollected++; 
        AchievementManager.instance.CheckAchievementsByType<SingleRun_PowerUp>(GetCurrentStats());
    }
    public void ResetSafeTime() => _currentSafeTime = 0;

    public RunStats GetCurrentStats()
    {
        //Debug.Log($"Current Robot ID: {GamePlayManager.instance.InGamePlayer.name}");
        return new RunStats
        {
            robotID = GamePlayManager.instance.InGamePlayer.name,
            coinsCollected = GamePlayManager.instance.TotalCoin,
            timeAlive = Time.timeSinceLevelLoad,
            powerUpsCollected = this.powerUpsCollected,
            maxContinuousSafeTime = this.maxContinuousSafeTime
        };
    }
}