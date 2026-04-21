using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Events Remove", menuName = "Event/Actions Functionality/Events Remove", order = 2)]
public class SOEventActionEventsRemove : SOEventAction
{
    public SOEvent[] EventsToRemove;

    public override void Execute(EventContext context)
    {
        context.ManagerEvents.EventRemoveList(EventsToRemove);
    }
}
