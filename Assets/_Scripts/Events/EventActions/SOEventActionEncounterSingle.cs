using UnityEngine;

[CreateAssetMenu(fileName = "Combat Single", menuName = "Event/Actions/Combat Single", order = 2)]
public class SOEventActionEncounterSingle : SOEventActionEncounter
{
    public CombatEncounter Encounter;

    protected override CombatEncounter GetEncounter()
    {
        return Encounter;
    }
}
