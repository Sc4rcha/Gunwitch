using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dialogue")]
    public class ActionDialogueDecision : FsmStateAction
    {
        public string OptionA;
        public string OptionB;
        public string OptionC;

        private string[] Options = new string[3];


        public override void OnEnter()
        {
            base.OnEnter();

            Options[0] = OptionA;
            Options[1] = OptionB;
            Options[2] = OptionC;

            ManagerGameElements.Instance.ManagerDialogue.DialogueDecisionSetup(Options);
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueDecision += TakeDecision;
        }

        public void TakeDecision(ManagerDialogue.DecisionOption option)
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueDecision -= TakeDecision;

            switch (option)
            {
                case ManagerDialogue.DecisionOption.OptionA:
                    Fsm.Event("DIALOGUE_OPTION_A");
                    break;
                case ManagerDialogue.DecisionOption.OptionB:
                    Fsm.Event("DIALOGUE_OPTION_B");
                    break;
                case ManagerDialogue.DecisionOption.OptionC:
                    Fsm.Event("DIALOGUE_OPTION_C");
                    break;
            }
        }
    }
}