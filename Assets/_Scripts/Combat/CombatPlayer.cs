using GameInfo;
using HutongGames.PlayMaker;
using System.Collections;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    [Header("Scene References")]
    public CombatGun Gun;

    public enum PlayerState { None, Shooting, Reloading, Consumable }
    public PlayerState State { get; private set; }


    public ManagerPlayer PlayerReference { get; private set; }


    private ManagerCombat manager;


    public void CombatStart(ManagerCombat manager)
    {
        // get references
        this.manager = manager;
        PlayerReference = ManagerGameElements.Instance.Player;

        // setup elements
        Gun.Setup(this);
    }
    public void CombatFinish()
    {

    }

    public void TurnStart()
    {
        // start turn by reloading gun
        ReloadStart();
    }
    public void TurnFinish()
    {
        ChangeState(PlayerState.None);

        // finish player round
        manager.PlayerRoundFinish();
    }

    #region Shooting
    public void AimOnOff() 
    {
        if (State == PlayerState.Shooting)
            PlayerHUDPortrait.Instance.GunAimOnOff(Gun.IsCursorOnShootingArea);
    }
    #endregion

    #region Reload
    public void ReloadStart() 
    {
        // change state to reaload
        ChangeState(PlayerState.Reloading);

    }
    public void ReloadRestart() 
    {
        Debug.Log("Restart");

        // stop coroutines in case script was waiting for last reload to go to aim
        StopAllCoroutines();

        ChangeState(PlayerState.Reloading);
    }
    public void ReloadFinish() 
    {
        // wait for reload animation to finish and set player portrait to aim
        StartCoroutine(LoadFinalBulletDelay());

        // lock bullet section inventory 
        manager.InventoryMenu.LockSection(ItemType.BULLET, true);
    }
    #endregion

    #region Consumable
    public void ConsumableStart()
    {
        ChangeState(PlayerState.Consumable);
    }
    public void ConsumableFinish()
    {
        ChangeState(PlayerState.Shooting);
    }
    #endregion


    private void ChangeState(PlayerState newState) 
    {
        // exit previous state
        switch (State)
        {
            case PlayerState.None:
                break;
            case PlayerState.Shooting:
                // player HUD exit aiming
                PlayerHUDPortrait.Instance.GunAimFinish();
                break;
            case PlayerState.Reloading:
                // player HUD exit reload
                PlayerHUDPortrait.Instance.ReloadFinish();

                // inventory exit reload
                manager.InventoryMenu.ReloadFinish();

                Gun.ReloadFinish();
                break;
            case PlayerState.Consumable:
                // player HUD exit reload
                PlayerHUDPortrait.Instance.ConsumableFinish();

                // inventory exit consumable
                manager.InventoryMenu.InventoryClose();
                break;
        }

        // enter new state
        switch (newState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Shooting:
                // player HUD aim or gun up
                PlayerHUDPortrait.Instance.GunAimOnOff(Gun.IsCursorOnShootingArea);
                break;
            case PlayerState.Reloading:
                // player HUD enter reload
                PlayerHUDPortrait.Instance.ReloadStart(Gun.BulletDefault.GetBullet());

                // inventory enter reload
                manager.InventoryMenu.ReloadStart();

                Gun.ReloadStart();
                break;
            case PlayerState.Consumable:
                // player HUD exit reload
                PlayerHUDPortrait.Instance.ConsumableStart();
                break;
        }

        State = newState;
    }


    // last bullet loaded delay before changing state
    private IEnumerator LoadFinalBulletDelay() 
    {
        yield return new WaitForSeconds(0.5f);

        ChangeState(PlayerState.Shooting);
    }
}
