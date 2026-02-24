using System;
using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RectTransform transitionCircle;

    [Header("Settings")]
    [SerializeField] private float transitionDuration = 0.6f; // Tổng thời gian chuyển cảnh
    [SerializeField] private float maxScale = 35f; // Tùy chỉnh độ lớn để che vừa đủ màn hình

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        transitionCircle.localScale = Vector3.zero;
    }

    // Hàm nhận một Action để thực thi đổi map ngay lúc màn hình bị che hoàn toàn
    public void PlayTransition(Action act)
    {
        StartCoroutine(TransitionRoutine(act));
    }

    private IEnumerator TransitionRoutine(Action act)
    {
        float halfDuration = transitionDuration / 2f;
        float elapsed = 0f;

        // Giai đoạn 1: Phóng to hình tròn
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float scale = Mathf.Lerp(0, maxScale, elapsed / halfDuration);
            transitionCircle.localScale = new Vector3(scale, scale, 1f);

            // Xoay nhẹ hình tròn để tạo cảm giác chuyển động giống video (Tùy chọn)
            transitionCircle.Rotate(0, 0, 150f * Time.unscaledDeltaTime);

            yield return null;
        }

        transitionCircle.localScale = new Vector3(maxScale, maxScale, 1f);

        // Giai đoạn 2: KHI MÀN HÌNH BỊ CHE KÍN -> Đổi môi trường
        act?.Invoke();

        // Giai đoạn 3: Thu nhỏ hình tròn
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float scale = Mathf.Lerp(maxScale, 0, elapsed / halfDuration);
            transitionCircle.localScale = new Vector3(scale, scale, 1f);

            transitionCircle.Rotate(0, 0, 150f * Time.unscaledDeltaTime);

            yield return null;
        }

        transitionCircle.localScale = Vector3.zero;
    }
}