using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Events Add", menuName = "Event/Actions Functionality/Events Add", order = 1)]
public class SOEventActionEventsAdd : SOEventAction
{
    public SOEvent[] EventsToAdd;

    public override void Execute(EventContext context)
    {
        context.ManagerEvents.EventAddList(EventsToAdd);
    }
}
