using UnityEngine;

[CreateAssetMenu(fileName = "Action Conditional", menuName = "Combat/Action/Conditional Hp")]
public class SOActionConditionalHp : SOActionConditional
{
    public int HpRangeMax;
    public int HpRangeMin;

    public override bool CheckConditional(CombatEncounter encounter, GameInfo.ActorEnemy enemy)
    {
        float hpPercentage = (float)enemy.HealthCurrent / (float)enemy.Health;

        return hpPercentage <= HpRangeMax && hpPercentage >= HpRangeMin;
    }
}
