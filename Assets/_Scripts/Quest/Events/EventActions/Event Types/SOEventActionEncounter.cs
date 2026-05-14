using GameInfo;
using UnityEngine;

public abstract class SOEventActionEncounter : SOEventAction
{
    public override void Execute(ManagerQuest manager)
    {
        void CombatFinish(CombatEndType endType) 
        {
            ManagerGameElements.Instance.OnCombatFinish -= CombatFinish;
            manager.EventFinish();
        }

        ManagerGameElements.Instance.CombatLoad(GetEncounter());
        ManagerGameElements.Instance.OnCombatFinish += CombatFinish;
    }

    protected abstract CombatEncounter GetEncounter();
}
