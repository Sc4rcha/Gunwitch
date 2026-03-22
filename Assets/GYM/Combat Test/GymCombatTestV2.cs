using GymCombat;
using UnityEngine;

public class GymCombatTestV2 : GymCombatTest
{
    public override void Setup()
    {
        base.Setup();
    }



    public override void RoundEnemyStart()
    {
        base.RoundEnemyStart();
    }
    public override void RoundEnemyFinish()
    {
        base.RoundEnemyFinish();
    }
    public override void EnemyTurnEnd()
    {
        base.EnemyTurnEnd();
    }



    public override void PlayerShoot()
    {
        if (Player.bulletsIndex == 3)
            RoundEnemyStart();
    }
    public override void PlayerReloadEnd()
    {

    }
}
