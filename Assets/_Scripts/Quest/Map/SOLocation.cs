using UnityEngine;

[CreateAssetMenu(fileName = "Location Info", menuName = "Map/Location")]
public class SOLocation : ScriptableObject
{
    public string Id;

    public string Name;
    public Sprite BackgroundSprite;
    [Space]
    [Tooltip ("Normal:show when event active, hide otherwise | ForceHide: always hidden | ForceShow: always visible")]
    public GameInfo.LocationVisibilityType InitialVisibility;

}
