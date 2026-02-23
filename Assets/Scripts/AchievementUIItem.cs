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

        if (isReady)
        {
            AchievementStage stage = data.stages[currentLv];

            if (completeTextObj != null) completeTextObj.SetActive(false);

            claimBtn.gameObject.SetActive(true);
            claimBtn.interactable = true;
            coinRewardIcon.gameObject.SetActive(true);
            rewardText.gameObject.SetActive(true);

            descriptionText.text = "Hoàn thành!";
            rewardText.text = stage.rewardCoins.ToString();

            claimBtn.onClick.AddListener(() => OnClaimClick(stage));
            return;
        }

        if (currentLv >= data.stages.Count)
        {
            if (completeTextObj != null) completeTextObj.SetActive(true);
            claimBtn.gameObject.SetActive(false);
            coinRewardIcon.gameObject.SetActive(false);
            rewardText.gameObject.SetActive(false);
            descriptionText.text = "Done!";
            return;
        }

        if (completeTextObj != null) completeTextObj.SetActive(false);

        claimBtn.gameObject.SetActive(true);
        claimBtn.interactable = false;
        coinRewardIcon.gameObject.SetActive(true);
        rewardText.gameObject.SetActive(true);

        AchievementStage nextStage = data.stages[currentLv];
        descriptionText.text = string.Format(data.descriptionTemplate, nextStage.targetValue);
        rewardText.text = nextStage.rewardCoins.ToString();
    }

    private void OnClaimClick(AchievementStage stage)
    {
        Debug.Log("Claim: " + data.title);
    }
}