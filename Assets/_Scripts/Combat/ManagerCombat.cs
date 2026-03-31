using System.Linq;
using UnityEngine;

public class ManagerCombat : MonoBehaviour
{
    public CombatPlayer Player;
    public Transform EncounterParent;
    public InventoryMenuCombat InventoryMenu;

    protected CombatEnounter encounter;
    protected int enemyTurnIndex;

    private void Start()
    {
        // register combat on combat scene load
        ManagerGameElements.Instance.ManagerEvents.CombatRegister(this);
    }
    public void CombatStart(CombatEnounter encounter) 
    {
        // instantiate and setup encounter
        this.encounter = Instantiate(encounter, EncounterParent);
        this.encounter.Setup(this);

        // setup combat inventory
        InventoryMenu.Setup(ManagerGameElements.Instance.Inventory, this);
        InventoryMenu.ShowSection(GameInfo.ItemType.BULLET);

        // setup player
        Player.Setup(this);

        // start combat
        if (Player.Stats.Dexterity >= this.encounter.Enemies[0].Stats.Dexterity)
            PlayerRoundStart();
        else
            EnemyRoundStart();
    }
    public void CombatFinish(bool isWin)
    {
        ManagerGameElements.Instance.ManagerEvents.CombatEnd(isWin);
    }

    #region Player
    public void PlayerRoundStart() 
    {
        // send player event to start turn
        Player.TurnStart();
    }
    public void PlayerRoundFinish() 
    {
        // Start enemy round
        EnemyRoundStart();
    }
    #endregion

    #region Enemies
    public void EnemyRoundStart() 
    {
        // set index to 0 to start going through all enemies
        enemyTurnIndex = 0;

        // start turn of first enemy
        EnemyTurnStart();
    }
    public void EnemyRoundFinish() 
    {
        // start enemy round
        PlayerRoundStart();
    }
    public void EnemyTurnStart() 
    {
        // enemy turn start, enemy turn end if they are dead
        if (!encounter.Enemies[enemyTurnIndex].Stats.IsDead)
            encounter.Enemies[enemyTurnIndex].TurnStart();
        else
            encounter.Enemies[enemyTurnIndex].TurnFinish();
    }
    public void EnemyTurnFinish() 
    {
        enemyTurnIndex++;

        CheckCombatOver();

        // if script has gone through all enemies end enemy round
        if (enemyTurnIndex == encounter.Enemies.Length)
            EnemyRoundFinish();
    }
    #endregion

    public void CheckCombatOver() 
    {
        // check player win all enemies dead
        if (encounter.CheckEncounterFinished())
            CombatFinish(true);

        // check player lose player is dead
        if (Player.Stats.IsDead)
            CombatFinish(false);
    }
}
