using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
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
