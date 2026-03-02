using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Image flashPanel;
    [SerializeField] private RectTransform blackOverlayCircle;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Transform circleContainer;

    [Header("General Settings")]
    [SerializeField] private float totalSequenceDuration = 4f;

    [SerializeField] private GameObject whiteImage;

    [Header("Circle Spawn Settings")]
    [SerializeField] private float initialSpawnInterval = 1f;
    [SerializeField] private float minSpawnInterval = 0.05f;
    [SerializeField] private float initialCircleSpeed = 100f;
    [SerializeField] private float maxCircleSpeed = 300f;

    private bool isSpawningCircles = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        blackOverlayCircle.localScale = Vector3.zero;
        var color = flashPanel.color;
        color.a = 0;
        flashPanel.color = color;
    }

    public void PlayTransition(Action act)
    {
        StartCoroutine(TransitionSequence(act));
    }

    private IEnumerator TransitionSequence(Action act)
    {
        //GamePlayManager.instance.SetIsPlay(false);

        AudioManager.instance.PlayTransitionSFX();

        yield return StartCoroutine(Flash(0.7f));

        isSpawningCircles = true;
        Coroutine spawnRoutine = StartCoroutine(SpawnCircles());

        float waitTime = totalSequenceDuration * 0.7f;
        yield return new WaitForSecondsRealtime(waitTime);

        isSpawningCircles = false;
        StopCoroutine(spawnRoutine);
        yield return StartCoroutine(BlackSequence(act));

        foreach (Transform child in circleContainer)
        {
            Destroy(child.gameObject);
        }

        AudioManager.instance.StopTransitionSFX();
        whiteImage.SetActive(false);
        GamePlayManager.instance.SetIsPlay(true);
    }

    private IEnumerator SpawnCircles()
    {
        float durationForRampUp = totalSequenceDuration * 0.7f;
        float startTime = Time.unscaledTime;

        while (isSpawningCircles)
        {
            float elapsed = Time.unscaledTime - startTime;
            float progress = elapsed / durationForRampUp;

            progress = Mathf.Clamp01(progress);

            float currentInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, progress * progress);
            float currentSpeed = Mathf.Lerp(initialCircleSpeed, maxCircleSpeed, progress * progress);

            GameObject newCircle = Instantiate(circlePrefab, circleContainer);
            newCircle.transform.localPosition = Vector3.zero;

            newCircle.GetComponent<Zoom>().Initialize(currentSpeed);

            yield return new WaitForSecondsRealtime(currentInterval);
        }
    }

    private IEnumerator BlackSequence(Action act)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        float maxBlackScale = 40f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float scale = Mathf.Lerp(0, maxBlackScale, elapsed / duration);
            blackOverlayCircle.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        blackOverlayCircle.localScale = new Vector3(maxBlackScale, maxBlackScale, 1f);

        yield return StartCoroutine(Flash(0.7f));

        act?.Invoke(); // có act khong, neu co thi chay.

        blackOverlayCircle.localScale = Vector3.zero;
    }

    private IEnumerator Flash(float duration)
    {
        float halfDuration = duration / 2f;
        float elapsed = 0f;
        Color c = flashPanel.color;

        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / halfDuration);
            flashPanel.color = c;
            yield return null;
        }
        c.a = 1f; flashPanel.color = c;

        elapsed = 0f;

        whiteImage.SetActive(true);

        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / halfDuration);
            flashPanel.color = c;
            yield return null;
        }
        c.a = 0f; flashPanel.color = c;
    }
}