using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_FullRepair : PowerUpItem
{
    public override void ApplyPowerUp(Player player)
    {
        player.limb.FullRepair();
        AudioManager.instance.PlayNotificationSFX();
        Debug.Log("Full Repair Power-Up Applied!");
    }
}
