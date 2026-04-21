using UnityEngine;
using static ManagerDialogue;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionDialogue : FsmStateAction
    {
        public SODialogue Dialogue;

        public override void OnEnter()
        {
            ManagerGameElements.Instance.ManagerDialogue.DialogueStart(Dialogue);
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish += DialogueEnd;
        }

        public void DialogueEnd()
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish -= DialogueEnd;
            Finish();
        }
    }
}