using GameInfo;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionCombat : FsmStateAction
    {
        public CombatEncounter Encounter;
        public bool LoseBehaviour;

        public override void OnEnter()
        {
            ManagerGameElements.Instance.CombatLoad(Encounter);
            ManagerGameElements.Instance.OnCombatFinish += CombatFinish;
        }

        public void CombatFinish (CombatEndType endType)
        {
            ManagerGameElements.Instance.OnCombatFinish -= CombatFinish;

            switch (endType)
            {
                case CombatEndType.Win:
                Fsm.Event("COMBAT_WIN");
                    break;
                case CombatEndType.Lose:
                Fsm.Event("COMBAT_LOSE");
                    break;
                case CombatEndType.Special:
                Fsm.Event("COMBAT_SPECIAL");
                    break;
            }
        }

    }
}

