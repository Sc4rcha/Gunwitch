using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM", menuName = "Event/Actions/FSM", order = 4)]
public class SOEventActionFSM : SOEventAction
{
    public PlayMakerFSM FSM;

    public override void Execute(EventContext context)
    {
        context.ManagerEvents.EventFSM(FSM);
    }
}
