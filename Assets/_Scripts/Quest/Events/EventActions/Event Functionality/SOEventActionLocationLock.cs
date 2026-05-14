using UnityEngine;

[CreateAssetMenu(fileName = "Location Lock", menuName = "Event/Actions Functionality/Location Lock", order = 35)]
public class SOEventActionLocationLock : SOEventAction
{
    public bool IsLocked;

    public override void Execute(ManagerQuest manager)
    {
        manager.ManagerMap.LocationLock(IsLocked);
    }
}
