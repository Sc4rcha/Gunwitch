using UnityEngine;

[CreateAssetMenu(fileName = "Location Move To", menuName = "Event/Actions Functionality/Location Move To", order = 34)]
public class SOEventActionLocationMoveTo : SOEventAction
{
    public SOLocation Location;

    public override void Execute(ManagerEvents manager)
    {
        ManagerGameElements.Instance.ManagerMap.LocationSetInfo(Location);
    }
}
