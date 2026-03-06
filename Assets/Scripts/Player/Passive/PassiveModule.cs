using UnityEngine;

public abstract class PassiveModule : ScriptableObject
{
    public abstract void ApplyPassive(GameObject player, GamePlayManager manager);
}