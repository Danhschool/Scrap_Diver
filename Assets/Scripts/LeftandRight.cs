using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftandRight : MonoBehaviour
{


    [Header("move info")]
    public float movementDistance = 1f; 
    public float movementSpeed = 1f;  

    private Vector3 startLocalPos;
    [SerializeField] private Rigidbody rb;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();

        startLocalPos = transform.localPosition;

        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        float offset = Mathf.Sin(Time.time * movementSpeed) * movementDistance;

        Vector3 targetPosition = startLocalPos + new Vector3(offset, 0f, 0f);

        Vector3 targetWorldPos = transform.parent.TransformPoint(targetPosition);

        rb.MovePosition(targetWorldPos);
    }
}
