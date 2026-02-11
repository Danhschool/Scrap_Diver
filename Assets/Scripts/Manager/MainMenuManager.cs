using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class RobotData
    {
        public string robotName;

        public GameObject robot;

        [Header("Description")]
        public string advantage;   
        public string disadvantage; 

        [Header("Price")]
        public int price;
        public bool isUnlocked;
    }

    public static MainMenuManager instance;

    [Header("Character Data")]
    [SerializeField] private RobotData[] robotList;


    [Header("Setting")]
    public float distanceBetweenChars = 5f;
    public float snapTime = 0.3f;
    public float dragSpeed = 1.0f;

    [Header("Shop")]
    private bool isShop = false;
    [SerializeField] private int selectedIndex = 0;
    public int totalCharacters = 0;
    //[SerializeField] private GameObject select;

    [Header("Parallax Background")]
    public Transform[] backgroundLayers;
    public float[] parallaxMultipliers;

    [Header("Background")]
    [SerializeField] private GameObject background;

    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private Dictionary<int, GameObject> claws = new Dictionary<int, GameObject>();

    private float duration = 0.5f;


    private Rigidbody rb;
    private float targetX;
    private float currentVelocity;
    private bool isDragging = false;
    private Vector3 startDragMousePos;
    private Vector3 startDragContainerPos;

    private float minX, maxX;

    Coroutine imgUpAndDown;


    public RobotData[] RobotList => robotList;
    public int SelectedIndex => selectedIndex;
    public GameObject Background => background;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        //StartStatus(); 
        SpawnRobotsInShop();
    }

    public void StartShop()
    {
        isShop = true;
        RefreshShopData();
        //SpawnRobotsInShop();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        targetX = transform.position.x;

        selectedIndex = DataManager.SelectedPlayerIndex;
        targetX = selectedIndex * -10f;
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        totalCharacters = transform.childCount;

        maxX = 0f;
        minX = -((totalCharacters - 1) * distanceBetweenChars);

        UpdateShopUI(DataManager.SelectedPlayerIndex);
    }
    public void StopShop()
    {
        isShop = false;

        MoveDownRobot();

        transform.position = new Vector3(0, 0, 0);
        //SpawnRobot(0);
    }

    void Update()
    {
        if (isShop)
        {
            HandleInput();
            MoveParallaxBackground();
        }
        if (Input.GetKey(KeyCode.N)) SetState(true);
        if (Input.GetKey(KeyCode.B)) SetState(false);
    }

    void FixedUpdate()
    {
        
    }

    void HandleInput()
    {
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref currentVelocity, snapTime);
        rb.MovePosition(new Vector3(newX, transform.position.y, transform.position.z));
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startDragMousePos = Input.mousePosition;
            startDragContainerPos = transform.position;
            currentVelocity = 0f;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            float deltaMouseX = (Input.mousePosition.x - startDragMousePos.x);
            float moveAmount = (deltaMouseX / Screen.width) * 20f * dragSpeed;

            float potentialTargetX = startDragContainerPos.x + moveAmount;

            targetX = Mathf.Clamp(potentialTargetX, minX - 2f, maxX + 2f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            SnapToNearest();

        }

    }

    void SnapToNearest()
    {
        int nearestIndex = Mathf.RoundToInt(targetX / -10);

        nearestIndex = Mathf.Clamp(nearestIndex, 0, totalCharacters - 1);

        targetX = nearestIndex * -10;

        selectedIndex = nearestIndex;

        DataManager.SelectedPlayerIndex = selectedIndex;
        //Debug.Log(selectedIndex);
        UpdateShopUI(selectedIndex);
    }

    private void UpdateShopUI(int value)
    {
        Main_UiManager.instance.UpdateAdvantageText(robotList[value].advantage);
        Main_UiManager.instance.UpdateDisadvantageText(robotList[value].disadvantage);
        if (robotList[value].isUnlocked) Main_UiManager.instance.UpdateSelectButtonText("null++");
        else Main_UiManager.instance.UpdateSelectButtonText(robotList[value].price.ToString());
        Main_UiManager.instance.UpdateCoinText();
    }

    public void RefreshShopData()
    {
        if (robotList == null) return;

        for (int i = 0; i < robotList.Length; i++)
        {
            var charData = robotList[i];

            if (i == 0)
            {
                charData.isUnlocked = true;
            }
            else
            {
                charData.isUnlocked = DataManager.GetCharacterUnlockState(charData.robotName);
            }
        }
    }

    public bool BuyCharacter(int index)
    {
        var charData = robotList[index];

        if (DataManager.TotalCoin >= charData.price)
        {
            DataManager.TotalCoin -= charData.price;

            DataManager.SetCharacterUnlockState(charData.robotName, true);

            charData.isUnlocked = true;

            Debug.Log($"{charData.robotName} : {DataManager.TotalCoin}");
            return true;
        }
        else
        {
            Debug.Log("Not enough Money!");
            return false;
        }
    }
    private void SpawnRobotsInShop()
    {
        int childCount = transform.childCount;
        //for (int i = childCount - 1; i >= 0; i--)
        //{
        //    DestroyImmediate(transform.GetChild(i).gameObject); 
        //}

        for (int i = 0; i < robotList.Length; i++)
        {
            SpawnRobot(i);
        }
    }

    private void SpawnRobot(int i)
    {
        var charData = robotList[i];
        if (charData.robot != null)
        {
            GameObject newRobot = Instantiate(charData.robot, transform);
            //GameObject newRobot = Instantiate(charData.robot);
            int a = i - DataManager.SelectedPlayerIndex;
            Debug.Log(DataManager.SelectedPlayerIndex);
            newRobot.transform.localPosition = new Vector3(a * distanceBetweenChars, 0, 0); //

            if(!claws.ContainsKey(i)) claws.Add(i, newRobot);

            newRobot.name = robotList[i].robotName;
        }
    }
    void MoveParallaxBackground()
    {
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            if (backgroundLayers[i] != null)
            {
                float parallaxX = transform.position.x * 5 * parallaxMultipliers[i];

                Vector3 bgPos = backgroundLayers[i].position;
                bgPos.x = parallaxX;
                backgroundLayers[i].position = bgPos;
            }
        }
    }
    public void MoveUpRobot()
    {
        //ClawController[] claws = FindObjectsOfType<ClawController>();
        int check = DataManager.SelectedPlayerIndex;

        //float anchorX = 0f;
        //if (claws.ContainsKey(check))
        //{
        //    anchorX = claws[check].transform.position.x;
        //}

        for (int i = 0; i < robotList.Length; i++)
        {
            ClawController myClaw = claws[i].GetComponentInChildren<ClawController>();
            float targetX = (i - check) * 10;

            Vector3 moveDir = new Vector3(targetX, 50, 0);

            myClaw.ClawPull(moveDir);
        }
        if (cameraManager != null)
        {
            cameraManager.MoveCamera(new Vector3(0, 50, 0));
        }
    }
    public void MoveDownRobot()
    {
        //ClawController[] claws = FindObjectsOfType<ClawController>();
        int check = DataManager.SelectedPlayerIndex;
        for (int i = 0; i < robotList.Length; i++)
        {
            ClawController myClaw = claws[i].GetComponentInChildren<ClawController>();

            float targetX = (i - check) * 40;

            Vector3 moveDir = new Vector3(targetX, 0, 0);

            myClaw.ClawPull(moveDir);

        }
        if (cameraManager != null)
        {
            cameraManager.MoveCamera(new Vector3(0, -50, 0));
        }
    }

    public void SetState(bool isDown)
    {
        Vector2 targetPos = isDown ? new Vector2(0, 1400) : new Vector2(0, -1400);

        if (imgUpAndDown != null)
        {
            StopCoroutine(imgUpAndDown);
        }

        // Bắt đầu coroutine mới
        imgUpAndDown = StartCoroutine(MoveRoutine(targetPos, Main_UiManager.instance.Img));
    }
    private IEnumerator MoveRoutine(Vector2 target, RectTransform targetImage)
    {
        Vector2 startPos = targetImage.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            t = Mathf.SmoothStep(0f, 1f, t);

            targetImage.anchoredPosition = Vector2.Lerp(startPos, target, t);

            yield return null; 
        }

        targetImage.anchoredPosition = target;
        imgUpAndDown = null;
    }
}
