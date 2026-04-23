using UnityEngine;

public abstract class SOEncounterWincon : ScriptableObject
{
    public abstract bool CheckWincon(CombatEncounter encounter);
}
