using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Event/Actions/Dialogue", order = 1)]
public class SOEventActionDialogue : SOEventAction
{
    public SODialogue Dialogue;

    public override void Execute(ManagerEvents manager)
    {
        void DialogueFinish() 
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish -= DialogueFinish;
            manager.EventFinish(true);
        }

        ManagerGameElements.Instance.ManagerDialogue.DialogueStart(Dialogue);
        ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish += DialogueFinish;
    }
}
