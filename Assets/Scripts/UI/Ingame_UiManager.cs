using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ingame_UiManager : MonoBehaviour
{
    public static Ingame_UiManager instance { get; private set; }

    [Header("In game UI")]
    [SerializeField] private TMP_Text distance_Txt;
    [SerializeField] private TMP_Text time_Txt;
    [SerializeField] private TMP_Text coin_Txt;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UpdateDistanceUI(int _distance)
    {
        distance_Txt.text = _distance.ToString() + " m"; 
    }
    public void UpdateCoinUI(int _coin) => coin_Txt.text = _coin.ToString();
}
