using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Action", menuName = "Combat/Action/Action")]
public class SOEnemyAction : ScriptableObject
{
    public SOEnemySkill Skill;
    public SOActionConditional[] Conditions;
    [Range (1,10)]
    public int Priority;

    public bool CheckConditions (CombatEncounter encounter, ActorEnemy enemy)
    {
        foreach (var condition in Conditions)
            if (!condition.CheckConditional (encounter, enemy))
                return false;

        return true;
    }
}
