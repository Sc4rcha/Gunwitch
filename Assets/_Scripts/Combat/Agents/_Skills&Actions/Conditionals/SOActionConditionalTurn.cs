using UnityEngine;

[CreateAssetMenu(fileName = "Action Conditional", menuName = "Combat/Action/Conditional Turns")]
public class SOActionConditionalTurn : SOActionConditional
{
    public int TurnStart;
    public int TurnRepeat;

    public override bool CheckConditional(CombatEncounter encounter, GameInfo.ActorEnemy enemy)
    {
        if (TurnRepeat == 0)
            return encounter.TurnNumber == TurnStart;
        else
            return encounter.TurnNumber >= TurnStart && (encounter.TurnNumber - TurnStart) % TurnRepeat == 0;
    }
}
