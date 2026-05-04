using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies Survived", menuName = "Combat/WinCon/Enemies Survived")]
public class SOEncounterWinconEnemiesSurvive : SOEncounterWincon
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
