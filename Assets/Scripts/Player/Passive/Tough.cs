using UnityEngine;

[CreateAssetMenu(fileName = "Tough", menuName = "Passives/Tough")]
public class Tough : PassiveModule
{
    public float scaleBoost = 1.15f;

    public override void ApplyPassive(GameObject player, GamePlayManager manager)
    {
        player.transform.localScale *= scaleBoost;
    }
}