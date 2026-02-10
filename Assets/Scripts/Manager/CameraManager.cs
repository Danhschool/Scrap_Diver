using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private Player player;

    private float rotationSpeed = 2f;

    private float limitAngle = 10f;

    [Header("Camera Settings")]
    public float moveDuration = 1.5f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private float shakeTime = .2f;

    private Coroutine coroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            StartCoroutine(ShakeCoroutine());
        }
    }
    private void FixedUpdate()
    {
        //if(transform.rotation.y > limitAngle || transform.rotation.y < limitAngle )
        RotateCamera();
    }
    public void Shake()
    {
        if (coroutine != null) 
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ShakeCoroutine());
    }
    private IEnumerator ShakeCoroutine()
    {
        ShakeValue(2f);
        
        yield return new WaitForSeconds(shakeTime) ;

        ShakeValue(0f);

        yield break;
    }

    private void RotateCamera()
    {
        if(player == null)
            return;

        float targetAngle = 0f;
        Direction currentDir = player.movement.CheckDirection();

        if (currentDir == Direction.left)
        {
            targetAngle = limitAngle;
        }
        else if (currentDir == Direction.right)
        {
            targetAngle = -limitAngle;
        }

        float currentY = virtualCamera.transform.eulerAngles.y;

        float newY = Mathf.LerpAngle(currentY, targetAngle, rotationSpeed * Time.deltaTime);

        virtualCamera.transform.eulerAngles = new Vector3(
            virtualCamera.transform.eulerAngles.x,
            newY,
            virtualCamera.transform.eulerAngles.z
        );
    }
    public void ShakeValue(float _value)
    {

        noise.m_AmplitudeGain = _value;
        noise.m_FrequencyGain = _value;
    }


    public void MoveCamera(Vector3 moveDelta)
    {
        StartCoroutine(MoveCameraRoutine(moveDelta));
    }

    IEnumerator MoveCameraRoutine(Vector3 moveDelta)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + moveDelta;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            float curveT = moveCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, targetPos, curveT);

            yield return null;
        }

        transform.position = targetPos;
    }
}
