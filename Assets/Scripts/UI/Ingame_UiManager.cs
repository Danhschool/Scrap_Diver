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

    [Header("Game Over Panel 2")]
    [SerializeField] private GameObject gameOver_Panel_2;
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
            GamePlayManager.instance.GamePause();

            gameOver_Panel_2.SetActive(true);
        }
        else
        {
            StartCoroutine(GamePlayManager.instance.Countdown(() =>
            {
                GamePlayManager.instance.GameResume();
                pause_Btn.SetActive(true);
            }));
            gameOver_Panel_2.SetActive(false);
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
        Debug.Log("qqq");
        Ui_Effect.OnClickExit(_img, this, ref isDown);
    }
    public void OnMainClick(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);

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
        Debug.Log("Coin");
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
    public void UpdateContinueWithCoin(bool _isOK)
    {
        if(_isOK) continueWithCoin.color = Color.yellow;
        else continueWithCoin.color = Color.red;
    }
    public void UpdateProgressCoinText(float _currentValue, float _maxValue)
    {
        progress_Coin_Txt.text = _currentValue.ToString() + "/" + _maxValue.ToString();
        explain_Coin_Txt.text = "EARN " + _maxValue + " COIN IN A SINGLE RUN";

    } 
    public void UpdateProgressTimeText(float _currentValue, float _maxValue)
    {
        progress_Time_Txt.text = _currentValue.ToString() + "/" + _maxValue.ToString() ;
        explain_Time_Txt.text = "FLY FOR " + _maxValue + " IN A SINGLE RUN";
    }
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
        if (DataManager.BestDistance > _value) bestDis_Endtxt.text = "Best: " + DataManager.BestDistance;
        else
        {
            distance_Endtxt.color = Color.yellow;
            bestDis_Endtxt.text = "New Best!";
            bestDis_Endtxt.color = Color.yellow;
        }
    }
    public void UpdateTime_Endtxt(float _value)
    {
        time_Endtxt.text = _value.ToString() + "s";
        if (DataManager.BestTime > _value) bestTime_Endtxt.text = "Best: " + DataManager.BestTime;
        else
        {
            time_Endtxt.color = Color.yellow;
            bestTime_Endtxt.text = "New Best!";
            bestTime_Endtxt.color = Color.yellow;
        }
    }
    public void UpdateCoin_Endtxt(float _value)
    {
        coin_Endtxt.text = _value.ToString() + "s";
        if (DataManager.BestTotalCoin > _value) bestCoin_Endtxt.text = "Best: " + DataManager.BestTotalCoin;
        else
        {
            coin_Endtxt.color = Color.yellow;
            bestCoin_Endtxt.text = "New Best!";
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
    #endregion
}
