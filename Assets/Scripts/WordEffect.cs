using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Spin,
    UpDown,
    None
}
public class WordEffect : MonoBehaviour
{
    public float speed = 20;

    [SerializeField] private EffectType effectType;

    [SerializeField] private float range = 15f;
    private Quaternion startRotation;
    private Vector3 startPosition;

    private Transform m_transform;
    private RectTransform m_rectTransform;

    private Coroutine coroutine;

    void Awake()
    {
        m_transform = transform;
        m_rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        if(coroutine != null) StopAllCoroutines();

        if (m_rectTransform != null) startPosition = m_rectTransform.anchoredPosition;

        startRotation = m_transform.localRotation;

        StartCoroutine(RunEffect());
    }
    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopAllCoroutines();
            coroutine = null;
        }

        if (m_rectTransform != null) m_rectTransform.anchoredPosition = startPosition;

        m_transform.localRotation = startRotation;
    }
    private void Start()
    {

    }

    private IEnumerator RunEffect()
    {
        float timeOffset = Time.unscaledTime;

        while (true)
        {
            float time = (Time.unscaledTime - timeOffset) * speed;
            float val = Mathf.Sin(time);

            if (effectType == EffectType.Spin)
            {
                float angle = val * range;
                m_transform.localRotation = startRotation * Quaternion.Euler(0, 0, angle);
            }
            else if (effectType == EffectType.UpDown)
            {
                float offsetY = val * range;
                m_rectTransform.anchoredPosition = startPosition + new Vector3(0, offsetY, 0);
            }

            yield return null;
        }
    }
}
