using UnityEngine;

public class PowerUp_Magnet : PowerUpItem
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private Color auraColor = Color.yellow;

    public override void ApplyPowerUp(Player player)
    {
        player.area.ActiveMagnet(20 ,duration);
        player.vfx.ShowAura(duration, auraColor);
        AudioManager.instance.PlayNotificationSFX();

        Debug.Log("Magnet PowerUp Applied");
    }
}