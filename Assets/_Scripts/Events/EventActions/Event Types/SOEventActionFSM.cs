using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM", menuName = "Event/Actions/FSM", order = 24)]
public class SOEventActionFSM : SOEventAction
{
    public PlayMakerFSM FSM;

    public override void Execute(ManagerEvents manager)
    {
        manager.InstantiateFSM(FSM);
    }
}
