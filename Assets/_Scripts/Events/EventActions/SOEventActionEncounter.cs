using GameInfo;
using UnityEngine;

public abstract class SOEventActionEncounter : SOEventAction
{
    public override void Execute(EventContext context)
    {
        void CombatFinish(bool isWin) 
        {
            ManagerGameElements.Instance.OnCombatFinish -= CombatFinish;
            context.ManagerEvents.EventFinish(isWin);
        }

        ManagerGameElements.Instance.CombatLoad(GetEncounter());
        ManagerGameElements.Instance.OnCombatFinish += CombatFinish;
    }

    protected abstract CombatEncounter GetEncounter();
}
