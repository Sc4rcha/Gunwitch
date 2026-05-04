using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Action Conditional", menuName = "Combat/Action/Conditional Always")]
public class SOActionConditionalAlways : SOActionConditional
{
    public override bool CheckConditional(CombatEncounter encounter, ActorEnemy enemy)
    {
        return true;
    }
}
