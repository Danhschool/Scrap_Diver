using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Coin : Obstacles
{
    [SerializeField] private float rotateSpeed = 5f;

    [Header("Coin Value")]
    [SerializeField] private int coinValue = 1;

    private bool isCollected = false;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();

        gameObject.transform.Rotate(new Vector3(0, 0, 90) * rotateSpeed * Time.deltaTime);
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        isCollected = false;
    }

    public void Collected()
    {
        GamePlayManager.instance.UpdateCoin(coinValue);

        isCollected = true;

        ObjectPool.instance.ReturnObject(gameObject);  
        //Destroy(gameObject);
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
