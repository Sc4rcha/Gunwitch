using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Info", menuName = "Combat/Enemy")]
public class SOCombatEnemy : ScriptableObject
{
    public string Name;
    public string Id;

    // combat stats
    [Header ("Enemy Stats")]
    public int Health;
    [Space]
    public int Armor;
    public int MagicResist;
    [Space]
    public int Strength;
    public int Magic;
    [Space]
    public int Accuracy;
    public int Speed;

    [Header("Actions")]
    public SOEnemyAction[] Actions;

    [Header("Agent Loot")]
    public ItemDrop[] Loot;

    public ActorEnemy GetCombatActor() 
    {
        return new ActorEnemy(this);
    }
}
