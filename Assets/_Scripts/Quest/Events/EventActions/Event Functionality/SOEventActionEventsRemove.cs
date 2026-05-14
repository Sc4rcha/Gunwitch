using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Events Remove", menuName = "Event/Actions Functionality/Events Remove", order = 32)]
public class SOEventActionEventsRemove : SOEventAction
{
    public SOEvent[] EventsToRemove;

    public override void Execute(ManagerQuest manager)
    {
        manager.EventDeactivateList(EventsToRemove);
    }
}
