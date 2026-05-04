using UnityEngine;

public abstract class SOActionConditional : ScriptableObject
{
    public abstract bool CheckConditional(CombatEncounter encounter, GameInfo.ActorEnemy enemy);
}
