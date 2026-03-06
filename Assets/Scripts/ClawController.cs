using System;
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
    [SerializeField] private HingeJoint hinge;

    private Transform robotRootTransform;
    private Rigidbody childRb;

    private Vector3 localPos;
    private bool isBeingPulled = false;
    private float swayTimer = 0f;

    private Coroutine coroutine;

    // Expose state so callers can avoid re-triggering an in-progress pull.
    public bool IsBeingPulled => isBeingPulled;

    void Start()
    {
        if (transform.parent != null)
        {
            robotRootTransform = transform.parent;
        }
        else
        {
            robotRootTransform = transform;
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

            if (robotRootTransform != null)
                targetPos = robotRootTransform.TransformPoint(targetPos);

            childRb.MovePosition(targetPos);

            float tilt = Mathf.Cos(swayTimer) * -5f;
            childRb.MoveRotation(Quaternion.Euler(0, 0, tilt));
        }
    }

    public void Drop()
    {
        hinge.connectedBody = null;
        Destroy(hinge);
    }

    public void ClawPull(Vector3 targetLocalPos)
    {
        Vector3 startLocalPos = robotRootTransform.localPosition;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = StartCoroutine(PullSequence(startLocalPos, targetLocalPos));
    }

    IEnumerator PullSequence(Vector3 startPos, Vector3 endPos)
    {
        isBeingPulled = true;
        childRb.interpolation = RigidbodyInterpolation.None;
        float elapsed = 0f;

        AudioManager.instance.PlayGearSFX();

        while (elapsed < pullDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pullDuration;
            float curveT = pullCurve.Evaluate(t);

            robotRootTransform.localPosition = Vector3.Lerp(startPos, endPos, curveT);

            yield return null;
        }

        AudioManager.instance.StopGearSFX();

        robotRootTransform.localPosition = endPos;
        swayTimer = 0f;
        childRb.interpolation = RigidbodyInterpolation.Interpolate;
        isBeingPulled = false;
        coroutine = null;
    }
}