using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Character Data")]
    [SerializeField] private RobotData[] robotList;

    [Header("Map Data")]
    [SerializeField] private MapData[] mapList;

    [Header("Background")]
    [SerializeField] private GameObject background;

    [Header("Layout Settings")]
    public float distanceBetweenChars = 40f;

    [SerializeField] public bool HasAffordableRobot { get; private set; }
    [SerializeField] public int MaxAffordableRobotIndex { get; private set; } = -1;
    [SerializeField] public bool HasAffordableCheckpoint { get; private set; }
    [SerializeField] public int MaxAffordableCheckpointIndex { get; private set; } = -1;

    [SerializeField] private CameraManager cameraManager;

    [Header("Core Controllers")]
    [SerializeField] private ShopScrollController scrollController;
    [SerializeField] private ShopRobotSpawner robotSpawner;
    [SerializeField] private UITransitionController uiTransition;

    private int selectedIndex = 0;

    public ShopScrollController ScrollController => scrollController;
    public ShopRobotSpawner RobotSpawner => robotSpawner;
    public UITransitionController UiTransition => uiTransition;

    public RobotData[] RobotList => robotList;
    public int SelectedIndex => selectedIndex;
    public GameObject Background => background;
    public MapData[] MapList => mapList;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        RefreshShopData();
        robotSpawner.SpawnRobotsInShop(robotList, distanceBetweenChars);

        SetState(false);

        AudioManager.instance.PlayRandomBGM();
        AudioManager.instance.StopWindSFX();
        Main_UiManager.instance.UpdateCoinText();

        scrollController.OnSelectedIndexChanged += UpdateShopUIFromScroll;
    }

    private void OnDestroy()
    {
        if (scrollController != null)
        {
            scrollController.OnSelectedIndexChanged -= UpdateShopUIFromScroll;
        }
    }

    public void StartShop()
    {
        //RefreshShopData();

        //scrollController.SetupScroll(selectedIndex, robotList.Length, distanceBetweenChars);

        //selectedIndex = DataManager.SelectedPlayerIndex;
        //scrollController.SetupScroll(selectedIndex, robotList.Length, distanceBetweenChars);

        //UpdateShopUI(DataManager.SelectedPlayerIndex);
        //isShop = true;
        RefreshShopData();

        scrollController.SetupScroll(DataManager.SelectedPlayerIndex, robotList.Length, 10f);

        UpdateShopUI(DataManager.SelectedPlayerIndex);
    }

    public void StopShop()
    {
        scrollController.StopScroll();
        MoveDownRobot();
    }

    private void UpdateShopUIFromScroll(int newIndex)
    {
        selectedIndex = newIndex;
        UpdateShopUI(selectedIndex);
    }

    private void UpdateShopUI(int value)
    {
        var data = robotList[value];

        Main_UiManager.instance.UpdateAdvantageText(data.advantage);
        Main_UiManager.instance.UpdateDisadvantageText(data.disadvantage);

        if (data.isUnlocked)
        {
            Main_UiManager.instance.UpdateSelectButtonText("null++");
        }
        else
        {
            if (DataManager.TotalCoin >= data.price)
                Main_UiManager.instance.UpdateSelectButtonText(data.price.ToString());
            else
                Main_UiManager.instance.UpdateSelectButtonText($"<color=red>{data.price}</color>");
        }
        Main_UiManager.instance.UpdateCoinText();
    }

    public void RefreshShopData()
    {
        if (robotList == null) return;

        for (int i = 0; i < robotList.Length; i++)
        {
            var charData = robotList[i];
            charData.isUnlocked = DataManager.GetCharacterUnlockState(charData.robotName);
        }
    }

    public bool BuyCharacter(int index)
    {
        var charData = robotList[index];

        if (DataManager.TrySpendCoin(charData.price))
        {
            DataManager.SetCharacterUnlockState(charData.robotName, true);
            charData.isUnlocked = true;

            if (robotSpawner.claws.ContainsKey(index))
            {
                robotSpawner.SetRobotSilhouette(robotSpawner.claws[index], false);
            }

            RunStats stats = new RunStats();
            stats.robotCount = DataManager.GetUnlockedRobotCount();

            AchievementManager.instance.CheckAchievementsByType<UnlockRobot>(stats);

            Debug.Log($"{charData.robotName} : {DataManager.TotalCoin}");
            return true;
        }

        Debug.Log("Not enough Money!");
        return false;
    }

    public void DropRobot()
    {
        int check = DataManager.SelectedPlayerIndex;
        if (robotSpawner.claws.ContainsKey(check))
        {
            ClawController claw = robotSpawner.claws[check].GetComponentInChildren<ClawController>();
            claw.Drop();
        }
    }

    public void MoveUpRobot()
    {
        int check = DataManager.SelectedPlayerIndex;

        for (int i = 0; i < robotList.Length; i++)
        {
            if (!robotSpawner.claws.ContainsKey(i)) continue;

            // Lấy script Claw của từng robot
            ClawController myClaw = robotSpawner.claws[i].GetComponentInChildren<ClawController>();

            // Tính toán tọa độ X và Y đích trong không gian Local của cha (Container)
            float targetX = (i - check) * 10f;
            Vector3 targetLocalPos = new Vector3(targetX, 50f, 0f);

            // Ra lệnh cho con robot đó tự bơi về vị trí đích
            myClaw.ClawPull(targetLocalPos);
        }

        if (cameraManager != null) cameraManager.MoveCamera(new Vector3(0, 50, 0));
    }

    public void MoveDownRobot()
    {
        int check = DataManager.SelectedPlayerIndex;

        for (int i = 0; i < robotList.Length; i++)
        {
            if (!robotSpawner.claws.ContainsKey(i)) continue;

            ClawController myClaw = robotSpawner.claws[i].GetComponentInChildren<ClawController>();

            float targetX = (i - check) * 40f;
            Vector3 targetLocalPos = new Vector3(targetX, 0f, 0f);

            myClaw.ClawPull(targetLocalPos);
        }

        if (cameraManager != null) cameraManager.MoveCamera(new Vector3(0, -50, 0));
    }

    public void SetState(bool isDown)
    {
        uiTransition.SetState(isDown, Main_UiManager.instance.Img);
    }
}