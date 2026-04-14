using UnityEngine;

[CreateAssetMenu(fileName = "Event Combat", menuName = "Event/Event Combat")]
public class SOEventCombat : SOEvent
{
    [Space]
    public CombatEnounter Encounter;
}
