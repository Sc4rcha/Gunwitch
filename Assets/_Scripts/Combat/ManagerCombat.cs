using GameInfo;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ManagerCombat : MonoBehaviour
{
    [Header ("References Scene")]
    public CombatPlayer Player;
    public Transform EncounterParent;
    public InventoryMenuCombat InventoryMenu;
    public Bounds ArenaBounds;

    public bool IsPlayerTurn { get; private set; }
    public CombatEnounter Encounter { get; private set; }

    protected int enemyTurnIndex;

    private void Start()
    {
        // register combat on combat scene load
        ManagerGameElements.Instance.ManagerEvents.CombatRegister(this);
    }
    public void CombatStart(CombatEnounter encounter)
    {
        // instantiate and setup encounter
        this.Encounter = Instantiate(encounter, EncounterParent);
        this.Encounter.Setup(this);

        // setup player
        Player.CombatStart(this);

        // setup combat inventory
        InventoryMenu.Setup(ManagerGameElements.Instance.Player.Info, this);

        // start combat
        PlayerRoundStart();
    }
    public void CombatFinish(bool isWin)
    {
        Player.CombatFinish();
        ManagerGameElements.Instance.ManagerEvents.CombatEnd(isWin);
    }

    #region Player
    public void PlayerRoundStart() 
    {
        IsPlayerTurn = true;

        // unlock inventory and open bullet menu
        InventoryMenu.Lock(false);

        // send player turn start to encounter
        Encounter.PlayerTurnStart();

        // send player event to start turn
        Player.TurnStart();
    }
    public void PlayerRoundFinish() 
    {
        IsPlayerTurn = false;

        // close invetory and lock IN THIS ORDER
        InventoryMenu.InventoryClose();
        InventoryMenu.Lock(true);

        // send player turn finish to encounter
        Encounter.PlayerTurnFinish();

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
        Encounter.Enemies[enemyTurnIndex].TurnStart();
    }
    public void EnemyTurnFinish() 
    {
        enemyTurnIndex++;

        CheckCombatOver();

        // if script has gone through all enemies end enemy round
        if (enemyTurnIndex == Encounter.Enemies.Count)
            EnemyRoundFinish();
        else
            EnemyTurnStart();

    }
    #endregion

    public void CheckCombatOver() 
    {
        // check player win all enemies dead
        if (Encounter.CheckEncounterFinished())
            CombatFinish(true);

        // check player lose player is dead
        if (Player.PlayerReference.Info.Actor.IsDead)
            CombatFinish(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireCube(ArenaBounds.center, ArenaBounds.size);
    }
#endif
}
