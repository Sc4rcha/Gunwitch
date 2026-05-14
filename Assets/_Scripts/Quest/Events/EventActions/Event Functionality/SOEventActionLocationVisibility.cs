using UnityEngine;

[CreateAssetMenu(fileName = "Location Visibility", menuName = "Event/Actions Functionality/Location Visibility", order = 35)]
public class SOEventActionLocationVisibility : SOEventAction
{
    public SOLocation Location;
    public GameInfo.LocationVisibilityType Visilibity;

    public override void Execute(ManagerQuest manager)
    {
        manager.ManagerMap.LocationChangeVisibility(Location, Visilibity);
    }
}
