using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EfectType
{
    Spin,
    UpDown,
    None
}
public class WordEffect : MonoBehaviour
{
    public float speed = 20;
    private int i = 1;

    [SerializeField] private EfectType efectType;
    private Transform m_transform;
    private RectTransform m_rectTransform;

    private float duration = 0.4f;

    void Awake()
    {
        m_transform = transform;
        m_rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        if(efectType == EfectType.Spin)
            StartCoroutine(Spinning());
        else if(efectType==EfectType.UpDown)
            StartCoroutine(UpAndDown());

    }


    private IEnumerator Spinning()
    {
        float elapsed = 0;

        float safeDuration = Mathf.Max(duration, 0.01f);
        while (elapsed < safeDuration)
        {
            elapsed += Time.deltaTime;
            m_transform.Rotate(0, 0, i * speed * Time.deltaTime);
            yield return null;
        }
        i *= -1;
        yield return StartCoroutine(Spinning());
    }
    private IEnumerator UpAndDown()
    {
        float elapsed = 0;

        float safeDuration = Mathf.Max(duration, 0.01f);
        while (elapsed < safeDuration)
        {
            elapsed += Time.deltaTime;
            m_rectTransform.anchoredPosition = new Vector2(50, i * speed * Time.deltaTime -50);
            yield return null;
        }
        i *= -1;
        yield return StartCoroutine(UpAndDown());
    }
}
