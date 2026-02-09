using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{
    [Header("Setting")]
    public float distanceBetweenChars = 5f;
    public float snapTime = 0.3f;
    public float dragSpeed = 1.0f;

    [Header("Debug Info")]
    public int selectedIndex = 0;
    public int totalCharacters = 0;

    private Rigidbody rb;
    private float targetX;
    private float currentVelocity;
    private bool isDragging = false;
    private Vector3 startDragMousePos;
    private Vector3 startDragContainerPos;

    private float minX, maxX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        targetX = transform.position.x;

        totalCharacters = transform.childCount;

        maxX = 0f;
        minX = -((totalCharacters - 1) * distanceBetweenChars);
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref currentVelocity, snapTime);
        rb.MovePosition(new Vector3(newX, transform.position.y, transform.position.z));
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startDragMousePos = Input.mousePosition;
            startDragContainerPos = transform.position;
            currentVelocity = 0f;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            float deltaMouseX = (Input.mousePosition.x - startDragMousePos.x);
            float moveAmount = (deltaMouseX / Screen.width) * 20f * dragSpeed;

            float potentialTargetX = startDragContainerPos.x + moveAmount;

            targetX = Mathf.Clamp(potentialTargetX, minX - 2f, maxX + 2f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            SnapToNearest();
        }
    }

    void SnapToNearest()
    {
        int nearestIndex = Mathf.RoundToInt(targetX / -distanceBetweenChars);

        nearestIndex = Mathf.Clamp(nearestIndex, 0, totalCharacters - 1);

        targetX = nearestIndex * -distanceBetweenChars;

        selectedIndex = nearestIndex;
        Debug.Log( selectedIndex);
    }
}
