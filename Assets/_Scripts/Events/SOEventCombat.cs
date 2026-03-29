using UnityEngine;

[CreateAssetMenu(fileName = "Event Combat", menuName = "Overworld/Event Combat")]
public class SOEventCombat : SOEvent
{
    [Space]
    public CombatEnounter Encounter;
}
