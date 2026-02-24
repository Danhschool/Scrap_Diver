using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    private RectTransform rectTransform;
    private float growthSpeed;
    private float maxScaleBeforeDestroy = 50f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.zero;
    }

    public void Initialize(float speed)
    {
        this.growthSpeed = speed;

        // GetComponent<Image>().color = new Color(1, 1, 1, Random.Range(0.5f, 1f));
    }

    private void Update()
    {
        float currentScale = rectTransform.localScale.x;
        currentScale += growthSpeed * Time.unscaledDeltaTime;
        rectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);

        if (currentScale > maxScaleBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
}
