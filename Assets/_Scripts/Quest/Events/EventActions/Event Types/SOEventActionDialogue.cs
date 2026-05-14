using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Event/Actions/Dialogue", order = 21)]
public class SOEventActionDialogue : SOEventAction
{
    public SODialogue Dialogue;

    public override void Execute(ManagerQuest manager)
    {
        void DialogueFinish() 
        {
            ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish -= DialogueFinish;
            manager.EventFinish();
        }

        ManagerGameElements.Instance.ManagerDialogue.DialogueStart(Dialogue);
        ManagerGameElements.Instance.ManagerDialogue.OnDialogueFinish += DialogueFinish;
    }
}
