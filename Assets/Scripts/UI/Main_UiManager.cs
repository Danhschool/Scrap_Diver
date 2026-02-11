using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_UiManager : MonoBehaviour
{
    public static Main_UiManager instance;

    [SerializeField] private RectTransform img;

    [Header("Coin")]
    [SerializeField] private Animator coinIconAnimator;
    [SerializeField] private TMP_Text coinText;

    [Header("Panels")]
    [SerializeField] private List<GameObject> panels;

    [Header("Shop Robot")]
    [SerializeField] private GameObject shopRobot_Panel;
    [SerializeField] private TMP_Text advantage_Txt;
    [SerializeField] private TMP_Text disadvantage_Txt;
    [SerializeField] private TMP_Text selectButtonText;

    private bool isDown = false;
    private Coroutine activeMoveCoroutine;

    public RectTransform Img => img;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

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
        DataManager.SelectedPlayerIndex = MainMenuManager.instance.SelectedIndex;

        Invoke("StartGame", 1f);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Scene_Lv1");
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

        MainMenuManager.instance.StartShop();

        StartCoroutine(Delay(.4f));

        MainMenuManager.instance.MoveUpRobot();

        Ui_Effect.SwitchToPanel(robotShop_Panel,panels);
    }
    public void OnMapClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this,ref isDown);

        GameObject map_Panel = panels.Find(x => x.name == ("Map_Panel"));

        MainMenuManager.instance.SetState(true);

        if(activeMoveCoroutine != null) StopCoroutine(activeMoveCoroutine);

        activeMoveCoroutine = StartCoroutine(DelaySwitchPanel(map_Panel, panels));
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

    #region Shop
    public void OnSelectPlayerClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);

        Ui_Effect.SwitchToPanel(main_Panel, panels);
        Debug.Log("Select Player");
    }
    public void OnSelectRobotClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        int index = DataManager.SelectedPlayerIndex;

        var data = MainMenuManager.instance.RobotList[index];

        if(data.isUnlocked)
        {
            advantage_Txt.text = data.advantage;
            disadvantage_Txt.text = data.disadvantage;

            GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
            Debug.Log(main_Panel.name);

            MainMenuManager.instance.MoveDownRobot();
            MainMenuManager.instance.StopShop();
            MainMenuManager.instance.Background.SetActive(true);

            Ui_Effect.SwitchToPanel(main_Panel, panels);
        }
        else
        {
            bool success = MainMenuManager.instance.BuyCharacter(index);

            if (success)
            {
                selectButtonText.text = "null";
            }
            else
            {
                
            }
        }
    }
    public void OnShopBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);

        MainMenuManager.instance.StopShop();
        MainMenuManager.instance.Background.SetActive(true);

        Ui_Effect.SwitchToPanel(main_Panel, panels);
    }

    #endregion

    #region Select map

    public void OnSelectMapClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        PageScroller pageScroller = gameObject.GetComponentInChildren<PageScroller>();
        
        DataManager.SelectedLevelIndex = pageScroller.CurrentLevelIndex;
        
        Debug.Log(DataManager.SelectedLevelIndex + "AAA");
        MainMenuManager.instance.SetState(false);

        if (activeMoveCoroutine != null) StopCoroutine(activeMoveCoroutine);
        activeMoveCoroutine = StartCoroutine(DelaySwitchPanel(main_Panel, panels));
    }
    public void OnMapBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);

        MainMenuManager.instance.SetState(false);

        if (activeMoveCoroutine != null) StopCoroutine(activeMoveCoroutine);
        activeMoveCoroutine = StartCoroutine(DelaySwitchPanel(main_Panel, panels));
    }

    #endregion

    #region Update UI
    public void UpdateImageTrans(Vector2 value) => img.anchoredPosition = value;
    public void UpdateAdvantageText(string _value) 
    { 
        advantage_Txt.color = Color.yellow;
        advantage_Txt.text = _value; 
    }
    public void UpdateDisadvantageText(string _value)
    {
        disadvantage_Txt.color = Color.red;
        disadvantage_Txt.text = _value;
    }
    public void UpdateSelectButtonText(string _value) => selectButtonText.text = _value;
    public void UpdateCoinText() => coinText.text = DataManager.TotalCoin.ToString();
    #endregion
    

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

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        MainMenuManager.instance.Background.SetActive(false);
    }
    IEnumerator DelaySwitchPanel(GameObject targetPanel, List<GameObject> _panels)
    {
        yield return new WaitForSeconds(.2f);

        Ui_Effect.SwitchToPanel(targetPanel, _panels);
    }
}
