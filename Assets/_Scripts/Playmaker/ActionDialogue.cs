using UnityEngine;
using static ManagerDialogue;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dialogue")]
    public class ActionDialogue : FsmStateAction
    {
        public SODialogue Dialogue;

        public override void OnEnter()
        {
            base.OnEnter();

            ManagerGameElements.Instance.ManagerDialogue.DialogueStart(Dialogue);
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinished += DialogueEnd;
        }

        public void DialogueEnd()
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinished -= DialogueEnd;
            Finish();
        }
    }
}