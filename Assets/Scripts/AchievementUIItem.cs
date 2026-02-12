using UnityEngine;
using UnityEngine.UI;
using TMPro; // Bắt buộc: Dùng thư viện này để điều khiển Text (TMP)

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
        if (data.icon != null) challIcon.sprite = data.icon;

        if (currentLv >= data.stages.Count)
        {
            if (completeTextObj != null) completeTextObj.SetActive(true);
            claimBtn.gameObject.SetActive(false);
            coinRewardIcon.gameObject.SetActive(false);
            rewardText.gameObject.SetActive(false);
            descriptionText.text = "Bạn đã hoàn thành!";
        }
        else
        {
            if (completeTextObj != null) completeTextObj.SetActive(false);

            claimBtn.gameObject.SetActive(true);
            coinRewardIcon.gameObject.SetActive(true);
            rewardText.gameObject.SetActive(true);

            AchievementStage stage = data.stages[currentLv];
            descriptionText.text = string.Format(data.descriptionTemplate, stage.targetValue);
            rewardText.text = stage.rewardCoins.ToString();

            claimBtn.onClick.RemoveAllListeners();
            claimBtn.onClick.AddListener(() => OnClaimClick(stage));
        }
    }

    private void OnClaimClick(AchievementStage stage)
    {
        Debug.Log("Claim: " + data.title);
    }
}