using UnityEngine;

[CreateAssetMenu(fileName = "Combat Group", menuName = "Event/Actions/Combat Group", order = 3)]
public class SOEventActionEncounterGroup : SOEventActionEncounter
{
    public CombatEncounter[] Encounters;

    protected override CombatEncounter GetEncounter()
    {
        return Encounters[Random.Range(0, Encounters.Length)];
    }
}
