using UnityEngine;

[CreateAssetMenu(fileName = "Location Info", menuName = "Map/Location")]
public class SOLocation : ScriptableObject
{
    public string Name;
    public Sprite BackgroundSprite;
    [Space]
    [Tooltip ("Normal:show when event active, hide otherwise | ForceHide: always hidden | ForceShow: always visible")]
    public LocationVisibilityType InitialVisibility;

    public enum LocationVisibilityType { Normal, ForceHide, ForceShow }

}
