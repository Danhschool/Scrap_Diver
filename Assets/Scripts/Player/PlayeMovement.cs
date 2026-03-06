using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
// using UnityEngine.InputSystem; // Nếu cần thiết

public enum Direction
{
    left,
    right,
    up,
    down,
    none
};

public class PlayeMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private DefaultInputActions inputActions;
    private CharacterController characterController;
    private Camera mainCamera;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Movement Info")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float slowRadius = 5f;
    [SerializeField] private float stopThreshold = 0.2f;
    [SerializeField] private float speed;

    [Header("Limit Bounds")]
    //[SerializeField] private float xLimit = 15f;
    //[SerializeField] private float topLimit = 8f;
    //[SerializeField] private float bottomLimit = 18f;
    [SerializeField] private float edgePadding = 0.05f;

    [Header("Smooth Settings")]
    [Range(0f, 0.5f)]
    [SerializeField] private float smoothTime = 0.1f;
    private Vector3 currentDir;
    private Vector3 currentDirVelocity;

    private float baseSmoothTime;
    private float baseMoveSpeed;
    private Vector3 baseScale;
    private float baseHeight;

    private bool isDragging = false;

    private Vector2 currentMousePos;

    public Vector2 moveInput { get; private set; }


    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        baseMoveSpeed = moveSpeed;
        speed = moveSpeed;
        baseSmoothTime = smoothTime;
        baseScale = transform.localScale;
        baseHeight = characterController.height;

        anim = GetComponentInChildren<Animator>();
        AssignInputEvents();
    }
    private void OnDestroy()
    {
        transform.DOKill();
        DOTween.Kill(gameObject);
    }

    private void Update()
    {
        if (!GamePlayManager.instance.IsPlaying) return;
        if (controls.Character.MousePress.WasPressedThisFrame())
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
                isDragging = true;
        }

        if (isDragging)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            ApplyMouseMovement(mouseWorldPos);
        }
        else
            ApplyMovement(moveInput);

        MoveAnimation();
        ApplyPositionClamping();
    }

    //private void FixedUpdate()
    //{
        
    //}
    private void ApplyMovement(Vector2 Input)
    {
        Vector3 targetDir = new Vector3(Input.x, 0f, Input.y).normalized;

        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, smoothTime);

        if (currentDir.magnitude > 0.1f)
        {
            characterController.Move(currentDir * speed * Time.deltaTime);
        }
    }
    private void ApplyMouseMovement(Vector3 targetPosition)
    {
        Vector3 offset = targetPosition - transform.position;
        float distance = offset.magnitude;
        Vector3 direction = offset.normalized;

        float targetIntensity = 1f;

        if (distance <= stopThreshold)
        {
            targetIntensity = 0f;
        }
        else if (distance < slowRadius)
        {
            targetIntensity = distance / slowRadius;
        }

        Vector3 targetVector = direction * targetIntensity;

        currentDir = Vector3.SmoothDamp(currentDir, targetVector, ref currentDirVelocity, smoothTime);

        if (currentDir.magnitude > 0.01f)
        {
            characterController.Move(currentDir * speed * Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopThreshold);
    }

    private void ApplyPositionClamping()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp(viewPos.x, edgePadding, 1f - edgePadding);
        viewPos.y = Mathf.Clamp(viewPos.y, edgePadding, 1f - edgePadding);

        transform.position = mainCamera.ViewportToWorldPoint(viewPos);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(currentMousePos);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            //Debug.Log("Mouse World Position: " + ray.GetPoint(distance));
            return ray.GetPoint(distance);

        }

        return transform.position;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Movement.performed += context =>
        {
            if (!isDragging) moveInput = context.ReadValue<Vector2>();
        };
        controls.Character.Movement.canceled += context =>
        {
            if (!isDragging) moveInput = Vector2.zero;
        };

        controls.Character.Mouse.performed += context => currentMousePos = context.ReadValue<Vector2>();

        //controls.Character.MousePress.performed += context =>
        //{
        //    isDragging = true;
        //};

        controls.Character.MousePress.canceled += context =>
        {
            isDragging = false;
            moveInput = Vector2.zero;
        };
    }

    private void MoveAnimation()
    {
        bool isMoving = currentDir.sqrMagnitude > 0.01f;
        anim.SetBool("isMove", isMoving);
        anim.SetBool("idle", !isMoving);

        if (isMoving)
        {
            anim.SetFloat("DirX", currentDir.x);
            anim.SetFloat("DirZ", currentDir.z);
        }
    }

    public Direction CheckDirection()
    {
        if(currentDir.x < -0.2f)
            return Direction.left;
        else if (currentDir.x > 0.2f)
            return Direction.right;
        else
            return Direction.none;
    }
    public void ApplyMovementPenalty(float healthRatio)
    {
        speed = Mathf.Max(baseMoveSpeed * healthRatio, baseMoveSpeed * 0.3f);
        smoothTime = baseSmoothTime + (0.4f * (1f - healthRatio));
    }

    public void ShrinkPlayer(float targetScaleRatio, float duration)
    {
        transform.DOKill();

        transform.DOScale(baseScale * targetScaleRatio, 0.5f).SetTarget(gameObject);
        characterController.height = baseHeight * targetScaleRatio;

        DOVirtual.DelayedCall(duration, () => {
            if (this == null || transform == null) return;

            transform.DOScale(baseScale, 0.5f).SetTarget(gameObject);
            characterController.height = baseHeight;
        }).SetTarget(gameObject);
    }
    public void SetMovementStats(float speedMulti, float smoothMulti)
    {
        moveSpeed *= speedMulti;
        speed = moveSpeed;
        smoothTime *= smoothMulti;

        baseMoveSpeed = moveSpeed;
        baseSmoothTime = smoothTime;
    }

    public void SetGravityStats(float gMulti)
    {
        float scale = GamePlayManager.instance.GameSpeed * gMulti;

        GamePlayManager.instance.UpdateGameSpeed(scale);
    }
}