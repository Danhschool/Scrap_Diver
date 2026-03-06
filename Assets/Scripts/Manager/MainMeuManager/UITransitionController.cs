using System.Collections;
using UnityEngine;

public class UITransitionController : MonoBehaviour
{
    private float duration = 1f;
    private Coroutine imgUpAndDown;
    private bool isFirstTime = true;

    public void SetState(bool isDown, RectTransform targetImage)
    {
        float value = Screen.height;
        Vector2 targetPos = isDown ? new Vector2(0, value) : new Vector2(0, -value);

        if (imgUpAndDown != null) StopCoroutine(imgUpAndDown);

        imgUpAndDown = StartCoroutine(MoveRoutine(targetPos, targetImage));
    }

    public IEnumerator MoveRoutine(Vector2 target, RectTransform targetImage)
    {
        float uiWidth = Screen.width;
        float uiHeight = Screen.height;

        targetImage.sizeDelta = new Vector2(uiWidth, uiHeight);
        Vector2 startPos = targetImage.anchoredPosition;
        float elapsedTime = 0f;

        if (isFirstTime) isFirstTime = false;
        else AudioManager.instance.PlayWhooshSFX();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            targetImage.anchoredPosition = Vector2.Lerp(startPos, target, t);
            yield return null;
        }

        targetImage.anchoredPosition = target;
        imgUpAndDown = null;
    }
}