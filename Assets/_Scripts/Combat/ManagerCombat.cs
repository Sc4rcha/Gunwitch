using System.Linq;
using UnityEngine;

public class ManagerCombat : MonoBehaviour
{
    public CombatPlayer Player;
    public CombatAgent[] Enemies;

    protected bool allEnemiesDead => Enemies.All(x => x.Stats.IsDead);
    protected int enemyTurnIndex;

    private void Start()
    {
        CombatStart(true);
    }

    public void CombatStart(bool isPlayerFirst) 
    {
        // setup encounter enemies
        foreach (var enemy in Enemies)
            enemy.Setup(this);

        // setup player
        Player.Setup(this);

        // start combat
        if (isPlayerFirst)
            PlayerRoundStart();
        else
            EnemyRoundStart();
    }

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
        if (!Enemies[enemyTurnIndex].Stats.IsDead)
            Enemies[enemyTurnIndex].TurnStart();
        else
            Enemies[enemyTurnIndex].TurnFinish();
    }
    public void EnemyTurnFinish() 
    {
        enemyTurnIndex++;

        CheckCombatOver();

        // if script has gone through all enemies end enemy round
        if (enemyTurnIndex == Enemies.Length)
            EnemyRoundFinish();
    }
    #endregion

    public void CheckCombatOver() 
    {
        // check player win all enemies dead
        if (allEnemiesDead)
            CombatOver(true);

        // check player lose player is dead
        if (Player.Stats.IsDead)
            CombatOver(false);
    }
    public void CombatOver(bool isWin)
    {
        if (isWin)
            Debug.Log("Win");
        else
            Debug.Log("Lose");
    }
}
