using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private LayerMask coinLayer;

    private Coroutine magnetCoroutine;
    private Collider[] coinBuffer = new Collider[20];

    public void ActiveMagnet(float radius, float duration)
    {
        if(magnetCoroutine != null)
        {
            StopCoroutine(magnetCoroutine);
        }
        magnetCoroutine = StartCoroutine(MagnetRoutine(radius, duration));
    }

    IEnumerator MagnetRoutine(float radius, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            int coinCount = Physics.OverlapSphereNonAlloc(transform.position, radius, coinBuffer, coinLayer);
            for (int i = 0; i < coinCount; i++)
            {
                Collider coinCollider = coinBuffer[i];
                Obstacle_Coin coin = coinCollider.GetComponent<Obstacle_Coin>();
                if (coin != null)
                {
                    coin.StartAttraction(transform);
                }
            }
            elapsed += .2f;
            yield return new WaitForSeconds(.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Obstacle_Coin coin = other.GetComponent<Obstacle_Coin>();
            if (coin != null)
            {
                Transform target = transform.parent != null ? transform.parent : transform;
                coin.StartAttraction(target);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            AudioManager.instance.PlaySlowWindSFX();
        }
    }
}
