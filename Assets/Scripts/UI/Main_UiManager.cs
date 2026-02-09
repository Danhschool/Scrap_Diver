using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Main_UiManager : MonoBehaviour
{
    [SerializeField] private Animator coinIconAnimator;

    [Header("Panels")]
    [SerializeField] private List<GameObject> panels;

    [Header("Shop Robot")]
    [SerializeField] private GameObject shopRobot_Panel;
    [SerializeField] private TMP_Text description_Txt;

    private bool isDown = false;
    private Coroutine activeMoveCoroutine;

    private void Awake()
    {
        if (coinIconAnimator == null)
        {
            coinIconAnimator = GetComponentInChildren<Animator>();
        }
    }

    #region Main Panels

    public void OnStartClick()
    {
        Debug.Log("OnStartClick");
        
        Door_Manager[] door = FindObjectsOfType<Door_Manager>();
        foreach (var d in door)
        {
            d.StartCoroutine(d.MoveDoor());
        }
    }

    public void OnSettingClick(Image img)
    {
        //Debug.Log("OnSettingClick");
        Ui_Effect.OnClickExit(img,this,ref isDown);
        
        GameObject setting_Panel = panels.Find(x => x.name == ("Setting_Panel"));
        Debug.Log(setting_Panel.name);

        Ui_Effect.SwitchToPanel(setting_Panel, panels);
    }


    public void OnRobotShopClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this,ref isDown);

        GameObject robotShop_Panel = panels.Find(x => x.name == ("ShopRobot_Panel"));
        Debug.Log(robotShop_Panel.name);

        Ui_Effect.SwitchToPanel(robotShop_Panel,panels);
    }
    public void OnMapClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this,ref isDown);

        GameObject map_Panel = panels.Find(x => x.name == ("Map_Panel"));
        Debug.Log(map_Panel.name);

        Ui_Effect.SwitchToPanel(map_Panel, panels);
    }
    public void OnChallengeClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject challenge_Panel = panels.Find(x => x.name == ("Challenge_Panel"));
        Debug.Log(challenge_Panel.name);

        Ui_Effect.SwitchToPanel(challenge_Panel, panels);
    }
    #endregion

    #region Setting Panel

    public void OnSoundClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);
        Debug.Log("OnSoundClick");
    }
    public void OnMusicClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this,   ref isDown);
        Debug.Log("OnMusicClick");
    }
    public void OnCreditClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);
        Debug.Log("OnCreditClick");
    }
    public void OnLanguageClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);
        Debug.Log("OnLanguageClick");
    }



    #endregion

    #region
    public void OnSelectPlayerClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);

        Ui_Effect.SwitchToPanel(main_Panel, panels);
        Debug.Log("Select Player");
    }


    #endregion

    #region Update UI
    public void UpdateDescriptionText(string _value) => description_Txt.text = _value;

    #endregion
    public void OnBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);

        Ui_Effect.SwitchToPanel(main_Panel, panels);
    }

    public void OnCoinIconClick()
    {
        coinIconAnimator.Play("Coin_Roll", 0, 0f);
    }

    public void OnClickDown(Image _img)
    {
        Ui_Effect.OnClickDown(_img, this, ref isDown);
    }
    public void OnClickExit(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
    }
}
