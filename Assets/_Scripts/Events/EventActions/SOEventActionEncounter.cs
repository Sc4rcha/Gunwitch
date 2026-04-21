using GameInfo;
using UnityEngine;

public abstract class SOEventActionEncounter : SOEventAction
{
    public override void Execute(ManagerEvents manager)
    {
        void CombatFinish(bool isWin) 
        {
            ManagerGameElements.Instance.OnCombatFinish -= CombatFinish;
            manager.EventFinish(isWin);
        }

        ManagerGameElements.Instance.CombatLoad(GetEncounter());
        ManagerGameElements.Instance.OnCombatFinish += CombatFinish;
    }

    protected abstract CombatEncounter GetEncounter();
}
