using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event/Event", order = 0)]
public class SOEvent : ScriptableObject
{
    public string Name;
    public SOLocation EventLocation;
    [Tooltip ("Does this event stay after the player has finished?")]
    public bool IsPersistent;
    [Tooltip ("Does this event play the moment the player enters the location?")]
    public bool IsAutoplay;

    [Space]
    public SOEventAction[] EventActions;
}
