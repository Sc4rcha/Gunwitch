using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    public SOEncounterWincon[] Wincons;
    [Space]
    public List <CombatAgent> Enemies;

    public int TurnNumber { get; private set; }


    // list of spawned enemies, after spawned the list is cleared
    private List<CombatAgent> enemiesToAdd;
    private List<CombatAgent> allEnemies;
    private Dictionary<int, int> enemyPriorities;

    private ManagerCombat manager;

    public void Setup(ManagerCombat manager) 
    {
        this.manager = manager;

        // instantiate lists
        enemiesToAdd = new List<CombatAgent>();
        allEnemies = new List<CombatAgent>();
        enemyPriorities = new Dictionary<int, int>();

        // setup encounter enemies and set priority
        foreach (var enemy in Enemies)
        {
            allEnemies.Add(enemy);
            enemy.Setup(manager);
        }
        UpdateSortingOrders();

        TurnNumber = 0;
    }


    private void UpdateSortingOrders()
    {
        enemyPriorities = new Dictionary<int, int>();

        foreach (var enemy in allEnemies)
        {
            if (enemyPriorities.ContainsKey(enemy.Priority))
                enemyPriorities[enemy.Priority]++;
            else
                enemyPriorities.Add(enemy.Priority, 1);

            enemy.SetOrderInLayer(enemy.Priority * 1000 + enemyPriorities[enemy.Priority] * 100);
        }
    }



    #region Player turn
    public void PlayerTurnStart() 
    {
        TurnNumber++;

        // add agents to add to enemies
        foreach (var enemy in enemiesToAdd)
            Enemies.Add(enemy);
        enemiesToAdd.Clear();

        // send player start event to all enemies
        foreach (var enemy in Enemies)
            enemy.PlayerTurnStart();
    }
    public void PlayerTurnFinish() 
    {
        // add agents to add to enemies
        foreach (var enemy in enemiesToAdd)
            Enemies.Add(enemy);
        enemiesToAdd.Clear();

        // send player end event to all enemies
        foreach (var enemy in Enemies)
            enemy.PlayerTurnFinish();
    }
    #endregion


    public void SpawnEnemy (CombatAgent enemy, Vector3 position)
    {
        // instantiate new agent
        enemiesToAdd.Add(Instantiate(enemy, transform));

        // set enemy position
        position.x = Mathf.Clamp(position.x, manager.ArenaBounds.min.x, manager.ArenaBounds.max.x);
        position.y = Mathf.Clamp(position.y, manager.ArenaBounds.min.y, manager.ArenaBounds.max.y);
        enemiesToAdd[enemiesToAdd.Count - 1].transform.position = position;

        // setup and add to encounter list
        enemiesToAdd[enemiesToAdd.Count - 1].Setup(manager);

        // add to all enemies and update sorting order
        allEnemies.Add(enemiesToAdd[enemiesToAdd.Count - 1]);
        UpdateSortingOrders();

        // enter player turn state if player acting
        if (manager.IsPlayerTurn)
            enemiesToAdd[enemiesToAdd.Count - 1].PlayerTurnStart();

    }
    public void SpawnEnemy(CombatAgent enemy, Vector3 position, out CombatAgent spawnedEnemy) 
    {
        SpawnEnemy(enemy, position);

        spawnedEnemy = enemiesToAdd[enemiesToAdd.Count - 1];
    }
    public void SpawnEnemy(CombatAgent enemy) 
    {
        SpawnEnemy(enemy, manager.ArenaBounds.center);
    }


    public void AddSubEnemyToEncounter(CombatAgent agent) 
    {
        allEnemies.Add(agent);
        UpdateSortingOrders();
    }


    public virtual bool CheckEncounterWincons() 
    {
        // if any wincons and return false if any of them are not met
        foreach (var wincon in Wincons)
            if (!wincon.CheckWincon(this))
                return false;

        // otherwise return true if there are wincons, otherwise return false
        return Wincons.Length > 0;
    }


    public CombatAgent CheckForEnemy(string id) 
    {
        // check for active encounter enemies
        foreach (var enemy in Enemies)
        {
            if (enemy.EnemyStatsReference.Id == id && !enemy.Actor.IsDead)
                return enemy;
        }
        // check enemies that have not been added to the active encoutner enemies
        foreach (var enemy in enemiesToAdd)
        {
            if (enemy.EnemyStatsReference.Id == id && !enemy.Actor.IsDead)
                return enemy;
        }

        return null;
    }
    public bool CheckEncounterFinished() 
    {
        // check of all enemies are dead
        foreach (var enemy in enemiesToAdd)
        {
            if (!enemy.Actor.IsDead)
                return false;
        }

        // check of all enemies are dead
        foreach (var enemy in Enemies)
        {
            if (!enemy.Actor.IsDead)
                return false;
        }

        return true;
    }

    // True while enemies are in the middle of animations. The combat manager waits for this to be false to change phases
    public bool CheckEnemiesDoingStuff() 
    {
        foreach (var enemy in Enemies)
            if (enemy.IsDoingStuff)
                return true;

        return false;
    }



#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Bounds ArenaBounds = new Bounds(new Vector3(2, 1.5f), new Vector3(10, 6));
        Handles.DrawWireCube(ArenaBounds.center, ArenaBounds.size);
    }
#endif
}
