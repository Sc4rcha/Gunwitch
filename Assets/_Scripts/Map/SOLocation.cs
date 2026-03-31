using UnityEngine;

[CreateAssetMenu(fileName = "Location Info", menuName = "Overworld/Location")]
public class SOLocation : ScriptableObject
{
    public string Name;
    public Sprite BackgroundSprite;
    [Space]
    [Tooltip ("This location will show on the map even if no events are available")]
    public bool IsPersistent;
}
