using UnityEngine;
using GameInfo;

public class CombatPlayer : MonoBehaviour
{
    public CombatActor Stats { get; private set; }


    [Header("Player Info")]
    public SOCombatEnemy DebugPlayerStats;

    [Header("Scene References")]
    public CombatGun Gun;


    private ManagerCombat manager;

    public void Setup(ManagerCombat manager) 
    {
        this.manager = manager;

        Stats = DebugPlayerStats.GetCombatActor();
        Stats.Startcombat();

        Gun.Setup(this);
    }


    public void Damage(int value)
    {
        Stats.HealthChange(-value);
    }


    public void TurnStart() 
    {
        // start turn by reloading gun
        Gun.ReloadStart();
    }
    public void TurnFinish() 
    {
        // lock gun
        Gun.SetState(CombatGun.GunState.Locked);

        // finish player round
        manager.PlayerRoundFinish();
    }
}
