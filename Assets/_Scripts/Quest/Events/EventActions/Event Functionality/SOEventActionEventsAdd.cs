using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Events Add", menuName = "Event/Actions Functionality/Events Add", order = 31)]
public class SOEventActionEventsAdd : SOEventAction
{
    public SOEvent[] EventsToAdd;

    public override void Execute(ManagerQuest manager)
    {
        manager.EventAddList(EventsToAdd);
    }
}
