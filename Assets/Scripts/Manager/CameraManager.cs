using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[DefaultExecutionOrder(50)]
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    [SerializeField] private Player player;

    private float rotationSpeed = 2f;

    private float limitAngle = 10f;

    [Header("Camera Settings")]
    public float moveDuration = .8f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private float shakeTime = .2f;

    private Coroutine coroutine;

    // ensure camera movement always starts from a fixed reference point
    private Coroutine moveCoroutine;
    private Vector3 cameraBasePosition;

    public Vector3 CameraBasePosition => cameraBasePosition;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // store the fixed base position for all camera moves
        cameraBasePosition = transform.position;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    //private void Update()
    //{ 
    //    if(Input.GetButtonDown("Jump"))
    //    {
    //        StartCoroutine(ShakeCoroutine());
    //    }
    //}
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
        if(player == null) return;

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
        // stop any ongoing camera move so we don't stack movements
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        // always start from the fixed base position (cameraBasePosition)
        moveCoroutine = StartCoroutine(MoveCameraRoutine(moveDelta));
    }

    IEnumerator MoveCameraRoutine(Vector3 moveDelta)
    {
        // start from the fixed base position instead of current transform.position
        Vector3 startPos = cameraBasePosition;
        Vector3 targetPos = cameraBasePosition + moveDelta;

        cameraBasePosition = targetPos;

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

        moveCoroutine = null;
    }
}
