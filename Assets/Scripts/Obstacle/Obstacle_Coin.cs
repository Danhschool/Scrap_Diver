using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Coin : Obstacles
{
    [SerializeField] private float rotateSpeed = 5f;

    [Header("Coin Value")]
    [SerializeField] private int coinValue = 1;

    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float eatDistance = 1f;

    private bool isAttracted = false;

    private bool isCollected = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        isAttracted = false;
        isCollected = false;
    }

    protected override void Update()
    {
        if (!isAttracted)
        {
            base.Update();
        }

        gameObject.transform.Rotate(new Vector3(0, 0, 90) * rotateSpeed * Time.deltaTime);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isCollected = false;
        isAttracted = false;
    }

    public void StartAttraction(Transform targetPlayer)
    {
        if (isAttracted || isCollected) return;
        isAttracted = true;

        transform.SetParent(null);

        StartCoroutine(MoveTowardsPlayer(targetPlayer));
    }

    private IEnumerator MoveTowardsPlayer(Transform target)
    {
        while (target != null && !isCollected)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) <= eatDistance)
            {
                Collected();
                yield break;
            }
            yield return null;
        }
    }

    public void Collected()
    {
        if (isCollected) return;
        isCollected = true;

        if (GamePlayManager.instance != null)
        {
            GamePlayManager.instance.UpdateCoin(coinValue);
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayCoinSFX();
        }

        ObjectPool.instance.ReturnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
        if (other.CompareTag("Player"))
        {
            Collected();
        }
    }
}