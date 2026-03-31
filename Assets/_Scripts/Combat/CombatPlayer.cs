using UnityEngine;
using GameInfo;

public class CombatPlayer : MonoBehaviour
{
    public CombatActor Actor { get; private set; }

    [Header("Scene References")]
    public CombatGun Gun;
    public PlayerHUD HUD;

    private ManagerCombat manager;

    public void CombatStart(ManagerCombat manager) 
    {
        this.manager = manager;

        // get player stats
        Actor = new CombatActor(ManagerGameElements.Instance.Player.Info);

        // setup elements
        Gun.Setup(this);
        HUD.Setup();
    }
    public void CombatFinish() 
    {
        // send stats information to player
        ManagerGameElements.Instance.Player.CombatFinish(Actor);
    }

    public void Damage(int value)
    {
        // apply damage
        Actor.HealthChange(-value);

        // refresh HUD
        HUD.Refresh(Actor.Stats);
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
