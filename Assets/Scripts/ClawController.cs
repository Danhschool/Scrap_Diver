using System.Collections;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [Header("Sway")]
    [SerializeField] private float swayDistance = 1f;
    [SerializeField] private float swaySpeed = 2f;

    [Header("Pull")]
    [SerializeField] private float pullDuration = 1.5f;
    [SerializeField] private AnimationCurve pullCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Setup")]
    [SerializeField] private Transform mainCamera;

    private Transform parentTransform;
    private Rigidbody childRb;

    private Vector3 localPos;
    private bool isBeingPulled = false;
    private float swayTimer = 0f;

    private Coroutine coroutine;

    void Start()
    {
        if (transform.parent != null)
        {
            parentTransform = transform.parent;
        }
        else
        {
            parentTransform = transform;
            Debug.LogError("ClawController");
        }

        childRb = GetComponent<Rigidbody>();
        childRb.isKinematic = true;
        childRb.interpolation = RigidbodyInterpolation.Interpolate;

        localPos = transform.localPosition;

        if (mainCamera == null && Camera.main != null)
            mainCamera = Camera.main.transform;
    }

    void FixedUpdate()
    {

        if (!isBeingPulled)
        {
            swayTimer += Time.fixedDeltaTime * swaySpeed;

            float offsetX = Mathf.Sin(swayTimer) * swayDistance;
            Vector3 targetPos = localPos + new Vector3(offsetX, 0f, 0f);

            if (parentTransform != null)
                targetPos = parentTransform.TransformPoint(targetPos);

            childRb.MovePosition(targetPos);

            float tilt = Mathf.Cos(swayTimer) * -5f;
            childRb.MoveRotation(Quaternion.Euler(0, 0, tilt));
        }
    }

    public void ClawPull(Vector3 moveDelta)
    {
        Vector3 startParentPos = parentTransform.position;
        Vector3 targetParentPos = moveDelta;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(PullSequence(startParentPos, targetParentPos));
    }

    IEnumerator PullSequence(Vector3 startPos, Vector3 endPos)
    {
        isBeingPulled = true;

        childRb.interpolation = RigidbodyInterpolation.None;

        float elapsed = 0f;

        while (elapsed < pullDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pullDuration;
            float curveT = pullCurve.Evaluate(t);

            parentTransform.position = Vector3.Lerp(startPos, endPos, curveT);

            yield return null;
        }

        parentTransform.position = endPos;

        swayTimer = 0f;
        childRb.interpolation = RigidbodyInterpolation.Interpolate;
        isBeingPulled = false;
    }
}