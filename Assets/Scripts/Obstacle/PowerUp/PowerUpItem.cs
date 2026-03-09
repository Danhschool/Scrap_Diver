using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpItem : Obstacles, IPowerUp
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            ApplyPowerUp(player);
            player.statsTracker.AddPowerUp();
            CancelInvoke();
            gameObject.SetActive(false); 
        }
    }
    public abstract void ApplyPowerUp(Player player);
}
