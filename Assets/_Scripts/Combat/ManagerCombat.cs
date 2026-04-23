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
        InventoryMenu.Setup(ManagerGameElements.Instance.Player.Info, this);

        // setup screens
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

        // send player turn finish to encounter
        Encounter.PlayerTurnFinish();

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
        PlayerRoundStart();
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

    public void EnemyAttack(SOAbility ability) 
    {
        float playerDexModifier = (float)(Player.PlayerReference.Info.Actor.Dexterity - 1) / ((float)(GameInfo.Actor.MaxAbilityScore - 1) * 2);
        int abilityHitChance = Mathf.RoundToInt(ability.Hit * (1f - playerDexModifier));

        if (Random.Range (0,100) < abilityHitChance)
        {
            Player.PlayerReference.Damage(ability.Damage);
        }
        else 
        {
            PlayerHUDPortrait.Instance.HitNumber.ShowMiss();
            // Miss / dodge
        }
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
        else if (Player.PlayerReference.Info.Actor.IsDead)
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

        // Start enemy round
        EnemyRoundStart();
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireCube(ArenaBounds.center, ArenaBounds.size);
    }
#endif
}
