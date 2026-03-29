using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionCombat : FsmStateAction
    {
        public CombatEnounter Encounter;

        public override void OnEnter()
        {
            ManagerGameElements.Instance.ManagerEvents.CombatStart(Encounter);
            ManagerGameElements.Instance.ManagerEvents.OnCombatFinish += CombatFinish;
        }

        public void CombatFinish (bool isPlayerWin)
        {
            if (isPlayerWin)
                Fsm.Event("COMBAT_WIN");
            else
                Fsm.Event("COMBAT_LOSE");
        }

    }
}

