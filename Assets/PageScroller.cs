using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class PageScroller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("setting")]
    [SerializeField] private RectTransform viewportRect;

    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private float snapSpeed = 10f;
    [SerializeField] private float dragSensitivity = 1f;

    private float pageHeight;
    private int currentLevelIndex = 0;
    private bool isDragging = false;
    private Vector2 targetPosition;
    private List<RectTransform> pages = new List<RectTransform>();

    public int CurrentLevelIndex => currentLevelIndex;

    void Start()
    {
        if (viewportRect == null || contentPanel == null)
        {
            //Debug.LogError("Panel");
            return;
        }

        pageHeight = viewportRect.rect.height;

        SetupPagesSize();

        targetPosition = contentPanel.anchoredPosition;
    }

    void SetupPagesSize()
    {
        pages.Clear();

        foreach (Transform child in contentPanel)
        {
            RectTransform rt = child.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pageHeight);
                pages.Add(rt);
            }
        }

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);
    }

    void Update()
    {

        if (!isDragging && contentPanel != null)
        {
            contentPanel.anchoredPosition = Vector2.Lerp(
                contentPanel.anchoredPosition,
                targetPosition,
                Time.deltaTime * snapSpeed
            );
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;

        Vector2 pos = contentPanel.anchoredPosition;
        pos.y += eventData.delta.y * dragSensitivity;
        contentPanel.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        CalculateClosestPage();
    }

    private void CalculateClosestPage()
    {
        float currentY = contentPanel.anchoredPosition.y;

        currentLevelIndex = Mathf.Clamp(
            Mathf.RoundToInt(currentY / pageHeight),
            0,
            pages.Count - 1
        );

        SnapToPage(currentLevelIndex);
    }

    public void SnapToPage(int index)
    {
        currentLevelIndex = index;

        float newY = currentLevelIndex * pageHeight;

        targetPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
    }
}