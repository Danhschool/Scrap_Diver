using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Door : Obstacles
{
    [SerializeField] private float speed;
    [SerializeField] private bool isLeft = true;
    private float moveTime = 1f;
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public IEnumerator MoveDoor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            
            if (isLeft)
            {
                transform.localPosition += Vector3.left * speed * Time.deltaTime;
            }
            else
            {
                transform.localPosition += Vector3.right * speed * Time.deltaTime;
            }
            yield return null;
        }
    }
}
