using UnityEngine;

[CreateAssetMenu(fileName = "QuickManeuvering", menuName = "Passives/Quick Maneuvering")]
public class QuickManeuvering : PassiveModule
{
    public float speedMultiplier = 1.3f;
    public float smoothMultiplier = 0.5f;

    public override void ApplyPassive(GameObject player, GamePlayManager manager)
    {
        var movement = player.GetComponent<PlayeMovement>();
        if (movement != null)
            movement.SetMovementStats(speedMultiplier, smoothMultiplier);
    }
}