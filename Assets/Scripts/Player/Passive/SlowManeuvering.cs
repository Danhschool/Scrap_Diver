using UnityEngine;

[CreateAssetMenu(fileName = "SlowManeuvering", menuName = "Passives/Slow Maneuvering")]
public class SlowManeuvering : PassiveModule
{
    public float speedMultiplier = 0.8f;
    public float smoothMultiplier = 2.0f;

    public override void ApplyPassive(GameObject player, GamePlayManager manager)
    {
        var movement = player.GetComponent<PlayeMovement>();
        if (movement != null)
            movement.SetMovementStats(speedMultiplier, smoothMultiplier);
    }
}