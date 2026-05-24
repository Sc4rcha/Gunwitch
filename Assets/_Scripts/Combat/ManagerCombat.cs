using System.Collections;
using UnityEditor;
using UnityEngine;
using GameInfo;

public class ManagerCombat : MonoBehaviour
{
    [Header ("References Scene")]
    public CombatPlayer Player;
    public Transform EncounterParent;
    public InventoryMenuCombat InventoryMenu;
    public CombatEffects Effects;
    [Header ("Screens")]
    public CombatScreenAttack ScreenAttack;
    public CombatScreenPhases ScreenPhases;
    public CombatScreenWin ScreenWin;
    [Header("Other")]
    public CombatHitMessage[] HitMessages;
    [Header ("Variables")]
    public Bounds ArenaBounds;
    [Space]
    public SOCombatConfig Configuration;

    public bool IsPlayerTurn { get; private set; }
    public CombatEncounter Encounter { get; private set; }

    private int enemyTurnIndex;
    private bool isCombatFinished;
    private CombatEndType combatEndType;

    private void Start()
    {
        // register combat on combat scene load
        ManagerGameElements.Instance.CombatRegister(this);
    }
    public void CombatStart(CombatEncounter encounter)
    {
        // instantiate and setup encounter
        Encounter = Instantiate(encounter, EncounterParent);
        Encounter.Setup(this);

        // setup player
        Player.CombatStart(this);

        // setup combat inventory
        InventoryMenu.Setup(ManagerGameElements.Instance.Player.Actor, this);

        // setup screens
        ScreenAttack.Setup(this);
        ScreenWin.Setup();

        // start combat
        PlayerRoundStart();
    }
    public void CombatFinish(CombatEndType endType)
    {
        isCombatFinished = true;

        // combat end setup
        Player.CombatFinish();
        InventoryMenu.Lock(true);

        StartCoroutine(CombatFinishDelay(endType));
    }

    public void CombatExit() 
    {
        // play world idle animation
        ManagerGameElements.Instance.Player.HUD.Portrait.WorldIdle();

        // COMBAT END
        ManagerGameElements.Instance.CombatEnd(combatEndType);
    }


    #region Player
    public void PlayerRoundStart() 
    {
        if (isCombatFinished)
            return;

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

        StartCoroutine(PhaseChangeToEnemy());
    }
    #endregion

    #region Enemies
    public void EnemyRoundStart() 
    {
        if (isCombatFinished)
            return;

        // set index to 0 to start going through all enemies
        enemyTurnIndex = 0;

        // set phase message
        ScreenPhases.ShowPhase("Enemy Phase!", 1);
        ScreenPhases.OnPhaseMessageFinish += EnemyTurnStart;
    }
    public void EnemyRoundFinish() 
    {
        // start enemy round
        StartCoroutine(PhaseChangeToPlayer());
    }
    public void EnemyTurnStart() 
    {
        // remove the method from the end phase message event
        ScreenPhases.OnPhaseMessageFinish -= EnemyTurnStart;

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


    public void EnemyAttack(ActorEnemy enemy, SOEnemySkill ability) 
    {
        float rn = CombatMethods.RandomPercent();

        // check hit
        if (!CombatMethods.CheckHit(rn, enemy, ability))
        {
            Player.PlayerReference.HUD.ShowHitMessage("MISS");
            return;
        }
        // check dodge
        if (CombatMethods.CheckDodge(rn, enemy, ability, Player.PlayerReference.Actor, Configuration))
        {
            Player.PlayerReference.HUD.ShowHitMessage("DODGE");
            return;
        }
        // check luck
        if (CombatMethods.LuckyDodge (Player.PlayerReference.Actor,Configuration))
        {
            Player.PlayerReference.HUD.ShowHitMessage("LUCKY DODGE");
            return;
        }

        // damage
        Player.PlayerReference.Damage((int)CombatMethods.DamageEnemyToPlayer(Player.PlayerReference.Actor, enemy, ability, Configuration));
    }

    public void CheckCombatOver() 
    {
        // check if encounter special condition is met
        if (Encounter.CheckEncounterWincons())
            CombatFinish(CombatEndType.Special);
        // check player win all enemies dead
        else if (Encounter.CheckEncounterFinished())
            CombatFinish(CombatEndType.Win);
        // check player lose player is dead
        else if (Player.PlayerReference.Actor.IsDead)
            CombatFinish(CombatEndType.Lose);
    }


    private IEnumerator CombatFinishDelay(CombatEndType endType) 
    {
        combatEndType = endType;

        yield return new WaitForSeconds(1.5f);

        // if player won show combat win screen. Just exit if lost combat (TO DO LATER)
        if (endType == CombatEndType.Win || endType == CombatEndType.Special)
            ScreenWin.ScreenShow();
        else
            CombatExit();
    }
    private IEnumerator PhaseChangeToEnemy()
    {
        while (Encounter.CheckEnemiesDoingStuff())
            yield return null;

        yield return new WaitForSeconds(1);

        // send player turn finish to encounter
        Encounter.PlayerTurnFinish();

        // Start enemy round
        EnemyRoundStart();
    }
    private IEnumerator PhaseChangeToPlayer()
    {
        while (Encounter.CheckEnemiesDoingStuff())
            yield return null;

        yield return new WaitForSeconds(2);

        // Start enemy round
        PlayerRoundStart();
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireCube(ArenaBounds.center, ArenaBounds.size);
    }
#endif
}
