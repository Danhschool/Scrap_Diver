using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUIItem : MonoBehaviour
{
    [Header("Data")]
    private AchievementData data;

    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image challIcon;
    [SerializeField] private Image coinRewardIcon;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Button claimBtn;
    [SerializeField] private GameObject completeTextObj;

    public void Setup(AchievementData _data)
    {
        this.data = _data;
        RefreshView();
    }

    public void RefreshView()
    {
        int currentLv = DataManager.GetAchievementLevel(data.id);
        bool isReady = DataManager.IsRewardReady(data.id);

        if (data.icon != null) challIcon.sprite = data.icon;

        claimBtn.onClick.RemoveAllListeners();

        int displayIndex = currentLv;
        if (displayIndex >= data.stages.Count)
        {
            displayIndex = data.stages.Count - 1;
        }

        AchievementStage displayStage = data.stages[displayIndex];

        //descriptionText.text = string.Format(data.descriptionTemplate, displayStage.targetValue);
        string localizedTemplate = data.descriptionTemplate;
        if (LanguageManager.Instance != null)
        {
            localizedTemplate = LanguageManager.Instance.GetText(data.descriptionTemplate);
        }

        descriptionText.text = string.Format(localizedTemplate, displayStage.targetValue);

        rewardText.text = displayStage.rewardCoins.ToString();

        if (currentLv >= data.stages.Count && !isReady)
        {
            if (completeTextObj != null) completeTextObj.SetActive(true);
            claimBtn.gameObject.SetActive(false);
            coinRewardIcon.gameObject.SetActive(false);
            rewardText.gameObject.SetActive(false);
            return;
        }

        if (isReady)
        {
            if (completeTextObj != null) completeTextObj.SetActive(false);
            claimBtn.gameObject.SetActive(true);
            claimBtn.interactable = true;
            coinRewardIcon.gameObject.SetActive(true);
            rewardText.gameObject.SetActive(true);

            claimBtn.onClick.AddListener(() => OnClaimClick(displayStage));
            return;
        }

        if (completeTextObj != null) completeTextObj.SetActive(false);

        claimBtn.gameObject.SetActive(true);
        claimBtn.interactable = false;
        coinRewardIcon.gameObject.SetActive(true);
        rewardText.gameObject.SetActive(true);
    }

    private void OnClaimClick(AchievementStage stage)
    {
        claimBtn.interactable = false;

        if (Main_UiManager.instance != null)
        {
            Main_UiManager.instance.PlayCoinRewardEffect(claimBtn.transform.position, () =>
            {
                ProcessClaimData(stage);
            });
        }
        else
        {
            ProcessClaimData(stage);
        }
    }

    private void ProcessClaimData(AchievementStage stage)
    {
        DataManager.AddTotalCoin(stage.rewardCoins);

        int currentLv = DataManager.GetAchievementLevel(data.id);
        DataManager.SetAchievementLevel(data.id, currentLv + 1);

        DataManager.DecreaseUnclaimedReward(data.id);

        if (Main_UiManager.instance != null)
        {
            Main_UiManager.instance.UpdateCoinText();
        }

        RefreshView();
    }
}