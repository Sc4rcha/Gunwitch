using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Event/Actions/Dialogue", order = 1)]
public class SOEventActionDialogue : SOEventAction
{
    public SODialogue Dialogue;

    public override void Execute(EventContext context)
    {
        void DialogueFinish() 
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish -= DialogueFinish;
            context.ManagerEvents.EventFinish();
        }

        ManagerGameElements.Instance.ManagerDialogue.DialogueStart(Dialogue);
        ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish += DialogueFinish;
    }
}
