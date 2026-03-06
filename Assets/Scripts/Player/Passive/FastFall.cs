using UnityEngine;

[CreateAssetMenu(fileName = "FastFall", menuName = "Passives/Fast Fall")]
public class FastFall : PassiveModule
{
    public float gravityMultiplier = 2.5f;

    public override void ApplyPassive(GameObject player, GamePlayManager manager)
    {
        var movement = player.GetComponent<PlayeMovement>();
        if (movement != null)
            movement.SetGravityStats(gravityMultiplier);
    }
}