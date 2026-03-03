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

    [Header("Challenge")]
    public Transform contentPanel;
    public GameObject itemPrefab;

    [Header("Setting")]
    [SerializeField] private Sprite[] music_Sprites;
    [SerializeField] private Sprite[] sound_Sprites;
    [SerializeField] private Image music_Icon;
    [SerializeField] private Image sound_Icon;

    [Header("Exclamation")]
    [SerializeField] private GameObject exclamation;

    [Header("Trans")]
    [SerializeField] private Image trans;

    private bool isDown = false;
    private Coroutine activeMoveCoroutine;
    private Coroutine startGame;

    // background delay coroutine reference to prevent races
    private Coroutine backgroundDelayCoroutine;

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
    private void OnEnable()
    {
        LanguageManager.OnLanguageChanged += RefreshLocalizedUI;
    }

    private void OnDisable()
    {
        LanguageManager.OnLanguageChanged -= RefreshLocalizedUI;
    }
    private void RefreshLocalizedUI()
    {
        if (MainMenuManager.instance != null && advantage_Txt != null && disadvantage_Txt != null)
        {
            int index = MainMenuManager.instance.SelectedIndex;
            if (index >= 0 && index < MainMenuManager.instance.RobotList.Length)
            {
                var data = MainMenuManager.instance.RobotList[index];
                if (data.isUnlocked)
                {
                    UpdateAdvantageText(data.advantage);
                    UpdateDisadvantageText(data.disadvantage);
                }
            }
        }

        GameObject challenge_Panel = panels.Find(x => x.name == ("Challenge_Panel"));
        if (challenge_Panel != null && challenge_Panel.activeInHierarchy)
        {
            GenerateListChallenge();
        }
    }


    #region Main Panels

    public void OnStartClick()
    {
        DataManager.SelectedPlayerIndex = MainMenuManager.instance.SelectedIndex;

        if(startGame != null) StopCoroutine(startGame);
        startGame = StartCoroutine(TransAnimStart());

        //Invoke("StartGame", 1.5f);

        //trans.gameObject.SetActive(true);

    }

    //private void StartGame()
    //{
    //    SceneManager.LoadScene("Scene_Lv1");
    //}


    public void OnSettingClick(Image img)
    {
        //Debug.Log("OnSettingClick");
        Ui_Effect.OnClickExit(img,this,ref isDown);

        GameObject setting_Panel = panels.Find(x => x.name == ("Setting_Panel"));
        Debug.Log(setting_Panel.name);

        int levelIndex = (int)VolumeController.instance.CurrentSFXLevel;
        sound_Icon.sprite = sound_Sprites[levelIndex];

        int index = (int)VolumeController.instance.CurrentBGMLevel;
        music_Icon.sprite = music_Sprites[index];

        Ui_Effect.SwitchToPanel(setting_Panel, panels);
    }


    public void OnRobotShopClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this,ref isDown);

        GameObject robotShop_Panel = panels.Find(x => x.name == ("ShopRobot_Panel"));
        Debug.Log(robotShop_Panel.name);

        MainMenuManager.instance.StartShop();

        StartBackgroundDelay(.4f);

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

        GenerateListChallenge();

        Debug.Log(challenge_Panel.name);

        MainMenuManager.instance.SetState(true);

        if (activeMoveCoroutine != null) StopCoroutine(activeMoveCoroutine);
        activeMoveCoroutine = StartCoroutine(DelaySwitchPanel(challenge_Panel, panels));
    }
    #endregion

    #region Setting Panel

    public void OnSoundClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);
        Debug.Log("OnSoundClick");

        VolumeController.instance.ToggleSFX();

        int levelIndex = (int)VolumeController.instance.CurrentSFXLevel;
        sound_Icon.sprite = sound_Sprites[levelIndex];
    }
    public void OnMusicClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this,   ref isDown);
        Debug.Log("OnMusicClick");

        VolumeController.instance.ToggleBGM();

        int levelIndex = (int)VolumeController.instance.CurrentBGMLevel;
        music_Icon.sprite = music_Sprites[levelIndex];
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

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.NextLanguage();
        }
    }
    public void OnSettingBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);
        Ui_Effect.SwitchToPanel(main_Panel, panels);
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

        //int index = DataManager.SelectedPlayerIndex;
        int index = MainMenuManager.instance.SelectedIndex;

        var data = MainMenuManager.instance.RobotList[index];

        if(data.isUnlocked)
        {
            //advantage_Txt.text = data.advantage;
            //disadvantage_Txt.text = data.disadvantage;

            advantage_Txt.text = LanguageManager.Instance.GetText(data.advantage);
            disadvantage_Txt.text = LanguageManager.Instance.GetText(data.disadvantage);

            GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
            Debug.Log(main_Panel.name);

            DataManager.SelectedPlayerIndex = index;

            //MainMenuManager.instance.MoveDownRobot();
            MainMenuManager.instance.StopShop();
            ShowBackground();

            Ui_Effect.SwitchToPanel(main_Panel, panels);
        }
        else
        {
            bool success = MainMenuManager.instance.BuyCharacter(index);

            if (success)
            {
                DataManager.SelectedPlayerIndex = index;
                DataManager.SetCharacterUnlockState(data.robotName, true);
                selectButtonText.text = "null";
            }
            else
            {
                if (coinText != null)
                {
                    StartCoroutine(ShakeUI(coinText, 0.5f, 10f));
                }
                AudioManager.instance.PlayWrongSFX();
            }
        }
    }
    public void OnShopBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        Debug.Log(main_Panel.name);

        MainMenuManager.instance.StopShop();
        ShowBackground();

        if (activeMoveCoroutine != null) StopCoroutine(activeMoveCoroutine);
        activeMoveCoroutine = StartCoroutine(DelaySwitchPanel(main_Panel, panels));
    }

    #endregion

    #region Select map

    public void OnSelectMapClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));
        PageScroller pageScroller = gameObject.GetComponentInChildren<PageScroller>();
        
        int index = pageScroller.CurrentLevelIndex;

        if (DataManager.CanPlayLevel(index))
        {
            DataManager.SelectedLevelIndex = index;

            UpdateCoinText();

            MainMenuManager.instance.SetState(false);
            if (activeMoveCoroutine != null) StopCoroutine(activeMoveCoroutine);
            activeMoveCoroutine = StartCoroutine(DelaySwitchPanel(main_Panel, panels));
        }
        else
        {
            bool success = DataManager.TryBuyLevel(index);
            if (success)
            {
                DataManager.SelectedLevelIndex = index;
                pageScroller.SnapToPage(index);
            }
            else
            {
                if(coinText != null) StartCoroutine(ShakeUI(coinText, 0.5f, 10f));
                AudioManager.instance.PlayWrongSFX();
            }
        }

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

    public void OnChallBackClick(Image img)
    {
        Ui_Effect.OnClickExit(img, this, ref isDown);

        GameObject main_Panel = panels.Find(x => x.name == ("Main_Panel"));

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
        //advantage_Txt.text = _value; 
        advantage_Txt.text = LanguageManager.Instance.GetText(_value);
    }
    public void UpdateDisadvantageText(string _value)
    {
        disadvantage_Txt.color = Color.red;
        //disadvantage_Txt.text = _value;
        disadvantage_Txt.text = LanguageManager.Instance.GetText(_value);
    }
    public void UpdateSelectButtonText(string _value) => selectButtonText.text = _value;

    /// ///////
    public void UpdateCoinText() => coinText.text = DataManager.TotalCoin.ToString();
    #endregion
    

    public void OnCoinIconClick()
    {
        coinIconAnimator.Play("Coin_Roll", 0, 0f);

        AudioManager.instance.PlayCoinSFX();
    }

    public void OnClickDown(Image _img)
    {
        Ui_Effect.OnClickDown(_img, this, ref isDown);
    }
    public void OnClickExit(Image _img)
    {
        Ui_Effect.OnClickExit(_img, this, ref isDown);
    }
    public void GenerateListChallenge()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        if (AchievementManager.instance == null) return;
        List<AchievementData> listData = AchievementManager.instance.AllAchievements;

        foreach (var data in listData)
        {
            GameObject newItem = Instantiate(itemPrefab, contentPanel);

            AchievementUIItem script = newItem.GetComponent<AchievementUIItem>();

            if (script != null)
            {
                script.Setup(data);
            }
        }
    }

    private void StartBackgroundDelay(float delayTime)
    {
        if (backgroundDelayCoroutine != null)
        {
            StopCoroutine(backgroundDelayCoroutine);
            backgroundDelayCoroutine = null;
        }

        backgroundDelayCoroutine = StartCoroutine(Delay(delayTime));
    }

    public void ShowBackground()
    {
        if (backgroundDelayCoroutine != null)
        {
            StopCoroutine(backgroundDelayCoroutine);
            backgroundDelayCoroutine = null;
        }

        MainMenuManager.instance.Background.SetActive(true);
    }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        MainMenuManager.instance.Background.SetActive(false);

        backgroundDelayCoroutine = null;
    }
    IEnumerator DelaySwitchPanel(GameObject targetPanel, List<GameObject> _panels)
    {
        yield return new WaitForSeconds(.2f);

        Ui_Effect.SwitchToPanel(targetPanel, _panels);
    }

    IEnumerator TransAnimStart()
    {
        Door_Manager[] door = FindObjectsOfType<Door_Manager>();
        foreach (var d in door)
        {
            d.StartCoroutine(d.MoveDoor());
        }

        MainMenuManager.instance.DropRobot();

        yield return new WaitForSeconds(1f);

        trans.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Scene_Lv1");
    }
    private IEnumerator ShakeUI(TMP_Text target, float duration, float magnitude)
    {
        float elapsed = 0.0f;
        target.color = Color.red;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            target.rectTransform.localPosition = new Vector2(x, y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.color = Color.white;
        target.rectTransform.localPosition = Vector2.zero;
    }
}
