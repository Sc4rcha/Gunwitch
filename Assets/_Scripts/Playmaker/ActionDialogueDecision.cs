using GameInfo;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionDialogueDecision : FsmStateAction
    {
        public string OptionA;
        public string OptionB;
        public string OptionC;

        private string[] Options = new string[3];


        public override void OnEnter()
        {
            Options[0] = OptionA;
            Options[1] = OptionB;
            Options[2] = OptionC;

            ManagerGameElements.Instance.ManagerDialogue.DialogueDecisionSetup(Options);
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueDecision += TakeDecision;
        }

        public void TakeDecision(DecisionOption option)
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueDecision -= TakeDecision;

            switch (option)
            {
                case DecisionOption.OptionA:
                    Fsm.Event("DIALOGUE_OPTION_A");
                    break;
                case DecisionOption.OptionB:
                    Fsm.Event("DIALOGUE_OPTION_B");
                    break;
                case DecisionOption.OptionC:
                    Fsm.Event("DIALOGUE_OPTION_C");
                    break;
            }
        }
    }
}