using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ingame_UiManager : MonoBehaviour
{
    public static Ingame_UiManager instance { get; private set; }

    private bool isDown = false;

    [Header("In game UI")]
    [SerializeField] private GameObject ingame_Panel;
    [SerializeField] private TMP_Text distance_Txt;
    //[SerializeField] private TMP_Text time_Txt;
    [SerializeField] private TMP_Text coin_Txt;
    [SerializeField] public GameObject pause_Btn;

    [Header("Progress Bar")]
    [SerializeField] Image fillProgressBar;
    [SerializeField] RectTransform arrowRect;

    [Header("Mark")]
    [SerializeField] private RectTransform mark_Container;
    [SerializeField] private GameObject mark;
    [SerializeField] private GameObject cup;

    [Header("Start Panel")]
    [SerializeField] private GameObject start_Panel;
    [SerializeField] private TMP_Text countDown_Txt;
    [SerializeField] private GameObject start_Btn;

    [Header("Continue Panel")]
    [SerializeField] private GameObject continue_Panel;
    [SerializeField] private Image fillContinueBar;
    [SerializeField] private Image continueWithCoin;
    [SerializeField] private TMP_Text continueWithCoin_txt;

    [Header("Game Over Panel 2")]
    [SerializeField] private GameObject pause_Panel;
    [SerializeField] private TMP_Text progress_Coin_Txt;
    [SerializeField] private TMP_Text explain_Coin_Txt;
    [SerializeField] private TMP_Text progress_Time_Txt;
    [SerializeField] private TMP_Text explain_Time_Txt;

    [Header("Game Over Panel 1")]
    [SerializeField] private GameObject gameOver_Panel;
    [SerializeField] private TMP_Text distance_Endtxt;
    [SerializeField] private TMP_Text bestDis_Endtxt;
    [SerializeField] private TMP_Text time_Endtxt;
    [SerializeField] private TMP_Text bestTime_Endtxt;
    [SerializeField] private TMP_Text coin_Endtxt;
    [SerializeField] private TMP_Text bestCoin_Endtxt;

    [Header("Setting Panel")]
    [SerializeField] private GameObject setting_Panel;
    [SerializeField] private Sprite[] music_Sprites;
    [SerializeField] private Sprite[] sound_Sprites;
    [SerializeField] private Image music_Icon;
    [SerializeField] private Image sound_Icon;

    [Header("Achievement Notification")]
    [SerializeField] private CanvasGroup challengePopup;

    Coroutine coroutineAch;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    #region OnClick

    public void OnStartClick()
    {   
        start_Btn.SetActive(false);

        GamePlayManager.instance.StartCountdown();
    }
    public void OnPauseClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
        if (GamePlayManager.instance.IsPlaying)
        {
            GamePlayManager.instance.PauseGame();

            pause_Panel.SetActive(true);

            UpdatePausePanel();
        }
        else
        {
            StartCoroutine(GamePlayManager.instance.Countdown(() =>
            {
                GamePlayManager.instance.ResumeGame();
                pause_Btn.SetActive(true);
            }));
            pause_Panel.SetActive(false);
            pause_Btn.SetActive(false);
        }
    }
    public void OnEndClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);

        Debug.Log("End");
    }

    public void OnSettingClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);

        int levelIndex = (int)VolumeController.instance.CurrentSFXLevel;
        sound_Icon.sprite = sound_Sprites[levelIndex];

        int index = (int)VolumeController.instance.CurrentBGMLevel;
        music_Icon.sprite = music_Sprites[index];

        pause_Panel.SetActive(false);
        setting_Panel.SetActive(true);
    }

    public void OnSoundClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        VolumeController.instance.ToggleSFX();

        int levelIndex = (int)VolumeController.instance.CurrentSFXLevel;
        sound_Icon.sprite = sound_Sprites[levelIndex];
    }
    public void OnMusicClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        VolumeController.instance.ToggleBGM();

        int levelIndex = (int)VolumeController.instance.CurrentBGMLevel;
        music_Icon.sprite = music_Sprites[levelIndex];
    }
    public void OnCreditClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);
    }
    public void OnLanguageClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.NextLanguage();
        }
    }
    public void OnSettingBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        pause_Panel.SetActive(true);
        setting_Panel.SetActive(false);
    }
    public void OnMainClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
        GamePlayManager.instance.ResumeGame();
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void OnClickDown(Image _img)
    {
        Ui_Effect.OnClickDown(_img, this, ref isDown);
    }
    public void OnClickExit(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
    }

    public void OnResumeWithCoinClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
        //Debug.Log("Coin");
        GamePlayManager.instance.ContinueWithCoin();
    }
    public void OnResumeWithAdClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
        Debug.Log("ad");
    }
    public void OnEndGameClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);

        GamePlayManager.instance.GameOver();
    }
    public void OnReStartClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    #endregion

    #region Set Active
    public void SetActiveIngame_Panel(bool _isActive) => ingame_Panel.SetActive(_isActive);
    public void SetActiveContinue_Panel(bool _isActive)
    {
        pause_Btn.SetActive(!_isActive);
        continue_Panel.SetActive(_isActive);
    }
    public void SetActiveStart_Panel(bool _isActive)  => start_Panel.SetActive(_isActive);
    public void SetActiveGameOver_Panel(bool _isActive) => gameOver_Panel.SetActive(_isActive);

    #endregion

    #region Update UI
    public void UpdatePausePanel()
    {
        if (AchievementManager.instance == null) return;
        List<AchievementData> listData = AchievementManager.instance.AllAchievements;

        foreach (var data in listData)
        {
            if (data.id != "coin" && data.id != "time") continue;

            int currentLevel = DataManager.GetAchievementLevel(data.id);
            int displayIndex = currentLevel >= data.stages.Count ? data.stages.Count - 1 : currentLevel;
            float targetValue = data.stages[displayIndex].targetValue;
            bool isComplete = currentLevel >= data.stages.Count;

            string currentValue = "";
            string descKey = "";
            string defaultDesc = "";
            TMPro.TMP_Text progressTxt = null;
            TMPro.TMP_Text explainTxt = null;

            if (data.id == "coin")
            {
                currentValue = GamePlayManager.instance.TotalCoin.ToString();
                descKey = "ach_desc_coin";
                defaultDesc = "EARN {0} COIN IN A SINGLE RUN";
                progressTxt = progress_Coin_Txt;
                explainTxt = explain_Coin_Txt;
            }
            else if (data.id == "time")
            {
                currentValue = GamePlayManager.instance.CurentTime.ToString();
                descKey = "ach_desc_time";
                defaultDesc = "FLY FOR {0}s IN A SINGLE RUN";
                progressTxt = progress_Time_Txt;
                explainTxt = explain_Time_Txt;
            }

            if (isComplete)
            {
                progressTxt.text = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ach_complete") : "COMPLETE";
            }
            else
            {
                progressTxt.text = currentValue + "/" + targetValue.ToString();
            }

            string localizedTemplate = LanguageManager.Instance != null ? LanguageManager.Instance.GetText(descKey) : defaultDesc;
            explainTxt.text = string.Format(localizedTemplate, targetValue);
        }
    }
    public void UpdateContinueWithCoin(bool _isOK)
    {
        if(_isOK) continueWithCoin.color = Color.yellow;
        else continueWithCoin.color = Color.red;
    }
    public void UpdateContinueWithCoin_Txt(int _value) => continueWithCoin_txt.text = "Coin: " + _value.ToString();
    //public void UpdateProgressCoinText(float _currentValue, float _maxValue)
    //{
    //    progress_Coin_Txt.text = _currentValue.ToString() + "/" + _maxValue.ToString();
    //    explain_Coin_Txt.text = "EARN " + _maxValue + " COIN IN A SINGLE RUN";

    //} 
    //public void UpdateProgressTimeText(float _currentValue, float _maxValue)
    //{
    //    progress_Time_Txt.text = _currentValue.ToString() + "/" + _maxValue.ToString() ;
    //    explain_Time_Txt.text = "FLY FOR " + _maxValue + " IN A SINGLE RUN";
    //}
    public void UpdateFillContinueBar(float _currentValue,float _maxValue) => fillContinueBar.fillAmount = _currentValue / _maxValue;
    public void UpdateFillProgressBar(float _currentValue , float _maxValue) => fillProgressBar.fillAmount = _currentValue / _maxValue;
    public void UpdateCountdown(string _text, bool _isActive)
    {
        countDown_Txt.gameObject.SetActive(_isActive);
        countDown_Txt.text = _text;
    }
    public void UpdateDistanceUI(int _distance) => distance_Txt.text = _distance.ToString() + " m"; 
    public void UpdateCoinUI(int _coin) => coin_Txt.text = _coin.ToString();
    public void UpdateArrowPosition(float _currentValue , float _maxValue)
    {
        float ratio = Mathf.Clamp01(_currentValue / _maxValue);

        AnchoedPosition(arrowRect, ratio);
    }
    public void UpdateProgressBar(float _currentValue, float _maxValue, int _level)
    {
        CreateCup(_currentValue, _maxValue);
        CreateMark(_level);
    }
    public void UpdateDistance_EndTxt(float _value)
    {
        distance_Endtxt.text = _value.ToString() + "m";
        if (DataManager.BestDistance > _value)
        {
            string localizedTemplate = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ui_best_format") : "Best: {0}";
            bestDis_Endtxt.text = string.Format(localizedTemplate, DataManager.BestDistance);
        }
        else
        {
            distance_Endtxt.color = Color.yellow;
            bestDis_Endtxt.text = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ui_new_best") : "New Best!";
            bestDis_Endtxt.color = Color.yellow;
        }
    }

    public void UpdateTime_Endtxt(float _value)
    {
        time_Endtxt.text = _value.ToString() + "s";
        if (DataManager.BestTime > _value)
        {
            string localizedTemplate = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ui_best_format") : "Best: {0}";
            bestTime_Endtxt.text = string.Format(localizedTemplate, DataManager.BestTime);
        }
        else
        {
            time_Endtxt.color = Color.yellow;
            bestTime_Endtxt.text = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ui_new_best") : "New Best!";
            bestTime_Endtxt.color = Color.yellow;
        }
    }

    public void UpdateCoin_Endtxt(float _value)
    {
        coin_Endtxt.text = _value.ToString();
        if (DataManager.BestTotalCoin > _value)
        {
            string localizedTemplate = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ui_best_format") : "Best: {0}";
            bestCoin_Endtxt.text = string.Format(localizedTemplate, DataManager.BestTotalCoin);
        }
        else
        {
            coin_Endtxt.color = Color.yellow;
            bestCoin_Endtxt.text = LanguageManager.Instance != null ? LanguageManager.Instance.GetText("ui_new_best") : "New Best!";
            bestCoin_Endtxt.color = Color.yellow;
        }
    }

    public void CreateCup(float _currentValue, float _maxValue)
    {
        float ratio = Mathf.Clamp01(_currentValue / _maxValue);

        GameObject gameObject = Instantiate(cup,mark_Container);
        RectTransform rect = gameObject.GetComponent<RectTransform>();

        AnchoedPosition(rect, ratio);
    }
    public void CreateMark(int _level)
    {
        if(_level < 1) return;
        
        for(int i = 1; i <= _level; i++)
        {
            float per = (float)i / _level;

            GameObject gameObject = Instantiate(mark, mark_Container);
            RectTransform rect = gameObject.GetComponent<RectTransform>();

            AnchoedPosition(rect, per);
        }
    }

    private void AnchoedPosition(RectTransform _rect, float _per)
    {
        _rect.anchorMin = new Vector2(_per, 0.5f);
        _rect.anchorMax = new Vector2(_per, 0.5f);

        _rect.anchoredPosition = new Vector2(0, _rect.anchoredPosition.y);
    }
    public void SetupDynamicProgressBar()
    {
        int startLevel = DataManager.SelectedLevelIndex + 1;

        int targetLevel = DataManager.LevelPassed + 1;

        int totalSegments = targetLevel - startLevel + 1;

        foreach (Transform child in mark_Container) Destroy(child.gameObject);

        float segmentWeight = 1f / totalSegments;
        for (int i = 1; i < totalSegments; i++)
        {
            float ratio = i * segmentWeight;
            GameObject m = Instantiate(mark, mark_Container);
            AnchoedPosition(m.GetComponent<RectTransform>(), ratio);
        }

        UpdateCupInDynamicWindow(startLevel, targetLevel, totalSegments);
    }

    private void UpdateCupInDynamicWindow(int startLv, int targetLv, int totalSeg)
    {
        float bestDist = DataManager.BestDistance;
        float startWindowDist = DataManager.GetStartDistance(startLv);
        float endWindowDist = DataManager.GetTargetDistance(targetLv);

        if (bestDist < startWindowDist) bestDist = startWindowDist;
        if (bestDist > endWindowDist) bestDist = endWindowDist;

        int bestLevel = targetLv;
        for (int i = startLv; i <= targetLv; i++)
        {
            if (bestDist <= DataManager.GetTargetDistance(i))
            {
                bestLevel = i;
                break;
            }
        }

        float segWeight = 1f / totalSeg;
        float sDist = DataManager.GetStartDistance(bestLevel);
        float tDist = DataManager.GetTargetDistance(bestLevel);

        float prog = 0f;
        if (tDist > sDist)
        {
            prog = Mathf.Clamp01((bestDist - sDist) / (tDist - sDist));
        }

        int indexInWindow = bestLevel - startLv;

        float cupRatio = (indexInWindow * segWeight) + (prog * segWeight);

        CreateCup(cupRatio, 1f);
    }
    public void ShowChallengeComplete()
    {
        if (coroutineAch != null) StopCoroutine(coroutineAch);
        coroutineAch = StartCoroutine(AnimateChallengeComplete());
        Debug.Log("Show Challenge Complete");
    }

    private IEnumerator AnimateChallengeComplete()
    {
        challengePopup.gameObject.SetActive(true);
        challengePopup.alpha = 1;
        float t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0, 1.2f, t / 0.3f);
            challengePopup.transform.localScale = Vector3.one * (scale > 1.1f ? 1f : scale);
            yield return null;
        }

        yield return new WaitForSeconds(.75f);

        t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            challengePopup.alpha = 1 - (t / 0.5f);
            yield return null;
        }
        challengePopup.alpha = 0;
        challengePopup.gameObject.SetActive(false);
    }
    #endregion
}
