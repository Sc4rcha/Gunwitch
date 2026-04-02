using UnityEngine;
using GameInfo;
using System.Collections;

public class CombatPlayer : MonoBehaviour
{
    [Header("Scene References")]
    public CombatGun Gun;

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


    #region Reload
    public void ReloadStart() 
    {
        // unlock inventory
        manager.InventoryMenu.Lock(false);

        // set inventory menu to bullet and lock consumables
        manager.InventoryMenu.ShowSection(ItemType.BULLET);
        manager.InventoryMenu.LockSection(ItemType.CONSUMABLE, true);

        Gun.ReloadStart();

        // Play player animation reload
        PlayerReference.HUD.Reload(Gun.BulletDefault.GetBullet());
    }
    public void ReloadRestart() 
    {
        // set inventory menu to bullet and lock consumables
        manager.InventoryMenu.ShowSection(ItemType.BULLET);
        manager.InventoryMenu.LockSection(ItemType.CONSUMABLE, true);

        // stop coroutines in case script was waiting for last reload to go to aim
        StopAllCoroutines();
    }
    public void ReloadFinish() 
    {
        // set inventory menu to consumables, unlock consumables and lock Bullets
        manager.InventoryMenu.ShowSection(ItemType.CONSUMABLE);
        manager.InventoryMenu.LockSection(ItemType.BULLET, true);
        manager.InventoryMenu.LockSection(ItemType.CONSUMABLE, false);

        Gun.ReloadFinish();

        // wait for reload animation to finish and set player portrait to aim
        StartCoroutine(LoadFinalBulletDelay());
    }
    #endregion

    public void TurnStart() 
    {
        // start turn by reloading gun
        ReloadStart();
    }
    public void TurnFinish() 
    {
        // lock gun
        Gun.ChangeState(CombatGun.GunState.Locked);

        // finish player round
        manager.PlayerRoundFinish();
    }


    private IEnumerator LoadFinalBulletDelay() 
    {
        yield return new WaitForSeconds(0.5f);
        // Player portrait AIM
        PlayerReference.HUD.Aim();
    }
}
