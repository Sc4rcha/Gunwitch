using GymCombat;
using UnityEngine;

public class GymCombatTestV3 : GymCombatTest
{
    public override void RoundEnemyStart()
    {

    }
    public override void RoundEnemyFinish()
    {
        base.RoundEnemyFinish();
    }
    public override void EnemyTurnStart()
    {
        // advance index
        enemyTurnIndex = (enemyTurnIndex + 1) % Enemies.Length;

        // enemy take turn
        if (!Enemies[enemyTurnIndex].Enemy.IsDead)
            Enemies[enemyTurnIndex].TurnStart();
        else
            EnemyTurnEnd();
    }
    public override void EnemyTurnEnd()
    {
        Player.isLocked = false;
        if (Player.bulletsIndex == 3)
            Player.ReloadStart();
    }


    public override void PlayerShoot()
    {
        Player.isLocked = true;
        EnemyTurnStart();
    }
    public override void PlayerReloadEnd()
    {

    }
}
