using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Only Enemies Alive", menuName = "Combat/Special Condition/Only Enemies Alive")]
public class SOEncounterWinconEnemyAlive : SOEncounterWincon
{
    public List <SOCombatEnemy> Enemies;

    public override bool CheckWincon(CombatEncounter encounter)
    {
        var requiredSet = new HashSet<string>(Enemies.Select(e => e.Id));

        foreach (var enemy in encounter.Enemies)
        {
            // get if the enemy should be dead anf if the enemy is dead or not
            bool shouldBeAlive = requiredSet.Contains(enemy.EnemyStatsReference.Id);
            bool isAlive = !enemy.Actor.IsDead;

            // compare
            if (isAlive != shouldBeAlive)
                return false;
        }

        return true;
    }
}
