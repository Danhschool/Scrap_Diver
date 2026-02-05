using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Ingame_UiManager : MonoBehaviour
{
    public static Ingame_UiManager instance { get; private set; }

    private bool isDown = false;

    [Header("In game UI")]
    [SerializeField] private TMP_Text distance_Txt;
    [SerializeField] private TMP_Text time_Txt;
    [SerializeField] private TMP_Text coin_Txt;

    [Header("Progress Bar")]
    [SerializeField] Image fillProgressBar;
    [SerializeField] RectTransform arrowRect;


    [Header("Mark")]
    [SerializeField] private RectTransform mark_Container;
    [SerializeField] private GameObject mark;

    [Header("Start Panel")]
    [SerializeField] private GameObject start_Panel;
    [SerializeField] private TMP_Text countDown_Txt;
    [SerializeField] private GameObject start_Btn;

    [Header("Continue Panel")]
    [SerializeField] private GameObject continue_Panel;
    [SerializeField] private Image fillContinueBar;

    [Header("Game Over Panel 2")]
    [SerializeField] private GameObject gameOver_Panel_2;    

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
    public void OnPauseClick()
    {
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
            }));
            gameOver_Panel_2.SetActive(false);
        }

    }

    public void OnSettingClick()
    {
        Debug.Log("qqq");
    }

    public void OnClickDown(Image _img)
    {
        Ui_Effect.OnClickDown(_img, this, ref isDown);
    }
    public void OnClickExit(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
    }

    #endregion

    #region Set Active Panel
    public void SetActiveContinue_Panel(bool _isActive) => continue_Panel.SetActive(_isActive);
    public void SetActiveStart_Panel(bool _isActive)  => start_Panel.SetActive(_isActive);

    #endregion

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
}
