using UnityEngine;

public class CombatEnounter : MonoBehaviour
{
    public CombatAgent[] Enemies;


    public void Setup(ManagerCombat manager) 
    {
        // setup encounter enemies
        foreach (var enemy in Enemies)
            enemy.Setup(manager);
    }

    public bool CheckEncounterFinished() 
    {
        // check of all enemies are dead
        foreach(var enemy in Enemies)
        {
            if (!enemy.Actor.IsDead)
                return false;
        }

        return true;
    }

}
