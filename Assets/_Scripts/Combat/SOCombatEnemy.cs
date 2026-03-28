using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Info", menuName = "Combat/Enemy")]
public class SOCombatEnemy : ScriptableObject
{
    public string Name;

    // combat stats
    [Header ("Combat Stats")]
    public int Health;
    public int Mana;

    [Header ("Agent Stat")]
    public int Body;
    public int Magic;
    public int Dexterity;
    public int Luck;
    public int Charisma;

    public CombatActor GetCombatActor() 
    {
        return new CombatActor(this);
    }
}
