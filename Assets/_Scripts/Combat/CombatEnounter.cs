using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatEnounter : MonoBehaviour
{
    public List <CombatAgent> Enemies;

    private List<CombatAgent> enemiesToAdd;
    private ManagerCombat manager;

    public void Setup(ManagerCombat manager) 
    {
        this.manager = manager;

        // setup encounter enemies
        foreach (var enemy in Enemies)
            enemy.Setup(manager);


        enemiesToAdd = new List<CombatAgent>();
    }

    #region Player turn
    public void PlayerTurnStart() 
    {
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
        foreach (var agent in enemiesToAdd)
            Enemies.Add(agent);
        enemiesToAdd.Clear();

        // send player end event to all enemies
        foreach (var enemy in Enemies)
            enemy.PlayerTurnFinish();
    }
    #endregion

    public void SpawnEnemy(CombatAgent enemy, Vector2 position, out CombatAgent spawnedEnemy) 
    {
        // instantiate new agent
        enemiesToAdd.Add(Instantiate(enemy, transform));
        spawnedEnemy = enemiesToAdd[enemiesToAdd.Count - 1];

        // set enemy position
        position.x = Mathf.Clamp(position.x, manager.ArenaBounds.min.x, manager.ArenaBounds.max.x);
        position.y = Mathf.Clamp(position.y, manager.ArenaBounds.min.y, manager.ArenaBounds.max.y);
        enemiesToAdd[enemiesToAdd.Count-1].transform.position = position;

        // setup and add to encounter list
        enemiesToAdd[enemiesToAdd.Count - 1].Setup(manager);
        
        // enter player turn state if player acting
        if (manager.IsPlayerTurn)
            enemiesToAdd[enemiesToAdd.Count - 1].PlayerTurnStart();

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


#if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        Bounds ArenaBounds = new Bounds(new Vector3(2, 1.5f), new Vector3(10, 6));
        Handles.DrawWireCube(ArenaBounds.center, ArenaBounds.size);
    }
#endif
}
