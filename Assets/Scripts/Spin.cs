using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spin : MonoBehaviour
{
    public float SpinSpeed = 10;

    private Transform m_transform;

    void Awake()
    {
        m_transform = transform;
    }
    // Update is called once per frame
    void Update()
    {
        m_transform.Rotate(0, 0, SpinSpeed * Time.deltaTime);
    }
}
