using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
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
    [SerializeField] private float xLimit = 15f;
    [SerializeField] private float topLimit = 8f;
    [SerializeField] private float bottomLimit = 18f;

    private bool isDragging = false;

    private Vector2 currentMousePos;

    public Vector2 moveInput { get; private set; }

    [Header("Smooth Settings")]
    [Range(0f, 0.5f)]
    [SerializeField] private float smoothTime = 0.1f;
    private Vector3 currentDir;
    private Vector3 currentDirVelocity;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        speed = moveSpeed;

        anim = GetComponentInChildren<Animator>();
        AssignInputEvents();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
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
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(currentPos.x, -xLimit, xLimit);

        currentPos.z = Mathf.Clamp(currentPos.z, -bottomLimit, topLimit);

        transform.position = currentPos;
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

        controls.Character.MousePress.performed += context => isDragging = true;

        controls.Character.MousePress.canceled += context =>
        {
            isDragging = false;
            moveInput = Vector2.zero;
        };
    }

    private void MoveAnimation()
    {
        anim.SetBool("idle", false);
        anim.SetBool("isMove", true);

        if (currentDir.x < -.2f)
        {
            anim.SetFloat("move", 0);
        }
        else if (currentDir.x > .2f)
        {
            anim.SetFloat("move", 1);
        }
        else if(currentDir.z > .2f)
        {
            anim.SetFloat("move", 2);
        }
        else if(currentDir.z < -.2f)
        {
            anim.SetFloat("move", 3);
        }
        else
        {
            anim.SetBool("isMove", false);
            anim.SetBool("idle", true) ;
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
}