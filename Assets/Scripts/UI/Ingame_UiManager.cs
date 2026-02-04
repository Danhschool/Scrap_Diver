using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ingame_UiManager : MonoBehaviour
{
    public static Ingame_UiManager instance { get; private set; }

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


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UpdateDistanceUI(int _distance) => distance_Txt.text = _distance.ToString() + " m"; 
    public void UpdateCoinUI(int _coin) => coin_Txt.text = _coin.ToString();

    public void UpdateFillProgressBar(float _currentValue , float _maxValue) => fillProgressBar.fillAmount = _currentValue / _maxValue;
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
