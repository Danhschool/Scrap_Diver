using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Shrink : PowerUpItem
{
    [SerializeField] private float targetScaleRatio = 0.5f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private Color auraColor = Color.blue;
    public override void ApplyPowerUp(Player player)
    {
        player.movement.ShrinkPlayer(targetScaleRatio, duration);
        player.vfx.ShowAura(duration, auraColor);
        AudioManager.instance.PlayNotificationSFX();
        Debug.Log("Applied Shrink Power-Up: Target Scale Ratio = " + targetScaleRatio + ", Duration = " + duration + " seconds");
    }
}
