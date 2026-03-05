using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Shield : PowerUpItem
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private Color auraColor = Color.red;

    public override void ApplyPowerUp(Player player)
    {
        player.limb.ActivateShield(duration);
        player.vfx.ShowAura(duration, auraColor);
        AudioManager.instance.PlayNotificationSFX();

        Debug.Log("Shield power-up applied to player for " + duration + " seconds.");
    }
}
