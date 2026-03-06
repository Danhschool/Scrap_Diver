using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShopScrollController : MonoBehaviour
{
    private float distanceBetweenChars = 5f;
    public float snapTime = 0.3f;
    public float dragSpeed = 1.0f;
    public float currentVelocity;

    public Action<int> OnSelectedIndexChanged;

    private Rigidbody rb;
    private float targetX;
    private bool isDragging = false;
    private Vector3 startDragMousePos;
    private Vector3 startDragContainerPos;

    private float minX, maxX;
    public int totalCharacters = 0;
    private bool isShop = false;
    private bool isScrollingSoundPlaying = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void SetupScroll(int selectedIndex, int total, float distance)
    {
        isShop = true;
        distanceBetweenChars = distance;
        totalCharacters = total;

        targetX = 0f;
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);

        // Giới hạn trượt dựa trên khoảng cách 10
        maxX = selectedIndex * distanceBetweenChars;
        minX = -(totalCharacters - 1 - selectedIndex) * distanceBetweenChars;
    }

    public void StopScroll()
    {
        isShop = false;
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (isShop)
        {
            HandleInput();
            PlaySound();
        }
    }

    public void HandleInput()
    {
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref currentVelocity, snapTime);
        rb.MovePosition(new Vector3(newX, transform.position.y, transform.position.z));

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

    public void SnapToNearest()
    {
        int offset = Mathf.RoundToInt(targetX / -distanceBetweenChars);

        int currentIndex = DataManager.SelectedPlayerIndex + offset;
        currentIndex = Mathf.Clamp(currentIndex, 0, totalCharacters - 1);

        targetX = offset * -distanceBetweenChars;

        OnSelectedIndexChanged?.Invoke(currentIndex);
    }

    public void PlaySound()
    {
        bool isMoving = Mathf.Abs(currentVelocity) > 5f;
        if (isMoving && !isScrollingSoundPlaying)
        {
            AudioManager.instance.PlayScrollSFX();
            isScrollingSoundPlaying = true;
        }
        else if (!isMoving && isScrollingSoundPlaying)
        {
            AudioManager.instance.StopScrollSFX();
            isScrollingSoundPlaying = false;
        }
    }
}