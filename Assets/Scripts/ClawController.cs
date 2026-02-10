using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ClawController : MonoBehaviour
{
    [Header("Sway")]
    [SerializeField] private float swayDistance = 1f;
    [SerializeField] private float swaySpeed = 2f;

    [Header("Pull")]
     //private float pullHeight = 50f;
    [SerializeField] private float pullDuration = 1.5f;
    [SerializeField] private AnimationCurve pullCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Setup")]
    [SerializeField] private Transform mainCamera; 
    private Rigidbody rb;

    private Vector3 startLocalPos;
    private bool isBeingPulled = false;
    private float swayTimer = 0f; 
    //private Vector3 camOffset;

    Coroutine coroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        startLocalPos = transform.localPosition;

        if (mainCamera == null && Camera.main != null)
            mainCamera = Camera.main.transform;

        //if (mainCamera != null)
        //    camOffset = mainCamera.position - transform.position;
    }

    void FixedUpdate()
    {
        if (!isBeingPulled)
        {
            swayTimer += Time.fixedDeltaTime * swaySpeed;

            float offsetX = Mathf.Sin(swayTimer) * swayDistance;
            Vector3 targetPos = startLocalPos + new Vector3(offsetX, 0f, 0f);

            if (transform.parent != null)
                targetPos = transform.parent.TransformPoint(targetPos);

            rb.MovePosition(targetPos);

            float tilt = Mathf.Cos(swayTimer) * -5f;
            rb.MoveRotation(Quaternion.Euler(0, 0, tilt));
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V) && !isBeingPulled)
    //    {
    //        coroutine = StartCoroutine(PullSequence(new Vector3(50, 50, 0)));
    //    }
    //}

    public void ClawPull(Vector3 moveDelta)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(PullSequence(moveDelta));
    }

    IEnumerator PullSequence(Vector3 moveDelta)
    {
        isBeingPulled = true;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + moveDelta;

        float elapsed = 0f;

        while (elapsed < pullDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pullDuration;
            float curveT = pullCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, targetPos, curveT);

            yield return null;
        }

        transform.position = targetPos;
        rb.position = targetPos;

        startLocalPos = transform.localPosition;
        swayTimer = 0f;
        isBeingPulled = false;
    }
}