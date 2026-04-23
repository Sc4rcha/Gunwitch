using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Info", menuName = "Combat/Enemy")]
public class SOCombatEnemy : ScriptableObject
{
    public string Name;
    public string Id;

    [Header("Sprites")]
    public Sprite AttackSprite;

    // combat stats
    [Header ("Combat Stats")]
    public int Health;
    public int Mana;

    [Header ("Agent Ability Scores")]
    public int Body;
    public int Magic;
    public int Dexterity;
    public int Luck;
    public int Charisma;

    [Header("Abilities")]
    public SOAbility Attack;

    [Header("Agent Loot")]
    public LootItem[] Loot;

    public Actor GetCombatActor() 
    {
        return new Actor(this);
    }
}
