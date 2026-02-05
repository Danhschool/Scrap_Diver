using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Ui_Effect
{
    //[System.Serializable]
    //public struct PanelItem {
    //    public string itemName;
    //    public GameObject panel_Obj;
    //}


    [Header("Icons")]
    private static Dictionary<RectTransform, Vector2> originalPositions = new Dictionary<RectTransform, Vector2>();

    public static float timeDown = .1f;
    public static float moveDistance = 10f;

    public static void OnClickDown(Image _img, MonoBehaviour _host,ref bool _isDown)
    {
        _host.StartCoroutine(MoveImgRoutine(_img, -1));

        _isDown = true;
        _img.color = Color.gray;

        //settingIcon.transform.position -= new Vector3(0, 10f, 0);
    }
    public static void OnClickExit(Image _img, MonoBehaviour _host,ref bool _isDown)
    {
        if (_isDown)
        {
            _host.StartCoroutine(MoveImgRoutine(_img, 1));
            _isDown = false;
            _img.color = Color.white;
            //settingIcon.transform.position -= new Vector3(0, 10f, 0);
        }
    }

    private static void StoreOriginalPos(RectTransform rect)
    {
        if (!originalPositions.ContainsKey(rect))
        {
            originalPositions.Add(rect, rect.anchoredPosition);
        }
    }
    //public static void OnClickExit(Image _img, MonoBehaviour _host, ref bool _isDown)
    //{
    //    _host.StartCoroutine(MoveImgRoutine(_img, 1));
    //    _isDown = true;

    //    _img.color = Color.white;
    //}

    private static IEnumerator MoveImgRoutine(Image _img, int _i)
    {
        if (_img == null) yield break;
        RectTransform rect = _img.rectTransform;

        StoreOriginalPos(rect);
        Vector2 origin = originalPositions[rect];

        Vector2 currentPos = rect.anchoredPosition;

        Vector2 targetPos = (_i == -1) ? origin + new Vector2(0, -moveDistance) : origin;

        float elapsed = 0;
        float safeDuration = Mathf.Max(timeDown, 0.01f);

        while (elapsed < safeDuration)
        {
            if (_img == null) yield break;
            elapsed += Time.unscaledDeltaTime;
            rect.anchoredPosition = Vector2.Lerp(currentPos, targetPos, elapsed / safeDuration);
            yield return null;
        }
        rect.anchoredPosition = targetPos;
    }

    public static void SwitchToPanel(GameObject targetPanel, List<GameObject> _panels)
    {
        DeactivateAllPanels(_panels);

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
    }
    private static void DeactivateAllPanels(List<GameObject> _panels)
    {
        //if (main_Panel != null) main_Panel.SetActive(false);
        //if (setting_Panel != null) setting_Panel.SetActive(false);
        //if (robotShop_Panel != null) robotShop_Panel.SetActive(false);
        //if (map_Panel != null) map_Panel.SetActive(false);
        //if (challenge_Panel != null) challenge_Panel.SetActive(false);

        foreach (var panel in _panels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }
}
