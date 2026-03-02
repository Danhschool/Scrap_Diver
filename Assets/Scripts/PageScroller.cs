using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class PageScroller : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI References")]
    [SerializeField] private RectTransform viewportRect;
    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private TMP_Text statusText;

    [Header("Dynamic Content")]
    [SerializeField] private GameObject[] pagePrefabs;

    [Header("Scroll Settings")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float dragSensitivity = 1f;
    [SerializeField] private float swipeThreshold = 0.15f;

    private float pageHeight;
    private int currentLevelIndex = 0;
    private bool isDragging = false;
    private Vector2 targetPosition;
    private Vector2 velocity = Vector2.zero;
    private float dragStartY;
    private int startIndexOnDrag;

    private List<RectTransform> pages = new List<RectTransform>();

    public int CurrentLevelIndex => currentLevelIndex;

    Coroutine coroutine;

    // Các biến hỗ trợ âm thanh
    private bool isScrollingSoundPlaying = false;
    //private float lastPosY;

    void Start()
    {
        if (viewportRect == null || contentPanel == null) return;

        Canvas.ForceUpdateCanvases();
        pageHeight = viewportRect.rect.height;

        GeneratePages();

        targetPosition = contentPanel.anchoredPosition;
        //lastPosY = targetPosition.y;

        UpdateLevelStatusUI();
    }

    private void OnEnable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SnapPage(DataManager.SelectedLevelIndex));
    }

    private void OnDisable()
    {
        currentLevelIndex = 0;

        targetPosition = new Vector2(contentPanel.anchoredPosition.x, 0);

        contentPanel.anchoredPosition = targetPosition;

        UpdateLevelStatusUI();

        // Ngắt âm thanh lập tức nếu UI đóng lại
        if (isScrollingSoundPlaying)
        {
            AudioManager.instance.StopScrollSFX();
            isScrollingSoundPlaying = false;
        }
    }

    IEnumerator SnapPage(int page)
    {
        yield return new WaitForSeconds(.5f);
        SnapToPage(page);
    }

    private void GeneratePages()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        pages.Clear();

        if (pagePrefabs == null || pagePrefabs.Length == 0) return;

        for (int i = 0; i < pagePrefabs.Length; i++)
        {
            if (pagePrefabs[i] != null)
            {
                GameObject newPage = Instantiate(pagePrefabs[i], contentPanel);

                RectTransform rt = newPage.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pageHeight);
                    pages.Add(rt);
                }

                TextMeshProUGUI distanceText = newPage.GetComponentInChildren<TextMeshProUGUI>();
                if (distanceText != null)
                {
                    int levelIndex = i + 1;
                    float targetDistance = DataManager.GetTargetDistance(levelIndex);
                    distanceText.text = targetDistance.ToString("0") + "m";
                }
            }
        }

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);
    }

    void Update()
    {
        if (!isDragging && contentPanel != null)
        {
            contentPanel.anchoredPosition = Vector2.SmoothDamp(
                contentPanel.anchoredPosition,
                targetPosition,
                ref velocity,
                smoothTime
            );

            if (isScrollingSoundPlaying && Mathf.Abs(velocity.y) < 10f)
            {
                AudioManager.instance.StopScrollSFX();
                isScrollingSoundPlaying = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragStartY = contentPanel.anchoredPosition.y;
        startIndexOnDrag = currentLevelIndex;

        if (!isScrollingSoundPlaying)
        {
            AudioManager.instance.PlayScrollSFX();
            isScrollingSoundPlaying = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = contentPanel.anchoredPosition;
        pos.y += eventData.delta.y * dragSensitivity;

        float maxY = 0;
        if (pages.Count > 0)
        {
            int maxAllowedIndex = Mathf.Min(DataManager.LevelPassed + 1, pages.Count - 1);

            maxY = maxAllowedIndex * pageHeight;
        }

        pos.y = Mathf.Clamp(pos.y, 0, maxY);

        contentPanel.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        CalculateTargetPage();
    }

    private void CalculateTargetPage()
    {
        float currentY = contentPanel.anchoredPosition.y;
        float dragDistance = currentY - dragStartY;

        if (Mathf.Abs(dragDistance) > (pageHeight * swipeThreshold))
        {
            if (dragDistance > 0)
            {
                currentLevelIndex = startIndexOnDrag + 1;
            }
            else
            {
                currentLevelIndex = startIndexOnDrag - 1;
            }
        }
        else
        {
            currentLevelIndex = Mathf.RoundToInt(currentY / pageHeight);
        }

        int maxAllowedIndex = Mathf.Min(DataManager.LevelPassed, pages.Count - 1);

        currentLevelIndex = Mathf.Clamp(currentLevelIndex, 0, maxAllowedIndex);

        SnapToPage(currentLevelIndex);
    }

    public void SnapToPage(int index)
    {
        currentLevelIndex = index;
        float newY = currentLevelIndex * pageHeight;
        targetPosition = new Vector2(contentPanel.anchoredPosition.x, newY);

        UpdateLevelStatusUI();
    }

    private void UpdateLevelStatusUI()
    {
        if (statusText == null) return;

        if (DataManager.CanPlayLevel(currentLevelIndex))
        {

            if (currentLevelIndex == DataManager.SelectedLevelIndex)
            {
                statusText.text = "OKKKKK";
            }
            else
            {
                statusText.text = "OK";
            }
        }
        else
        {
            int price = DataManager.GetLevelPrice(currentLevelIndex);
            if (price <= DataManager.TotalCoin)
            {
                statusText.text = price.ToString();

            }
            else
            {
                statusText.text = $"<color=red>{price}</color>";
            }
        }
    }
}