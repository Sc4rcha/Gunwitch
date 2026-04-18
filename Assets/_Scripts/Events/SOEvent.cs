using UnityEngine;

public class SOEvent : ScriptableObject
{
    public string Name;
    public SOLocation EventLocation;
    [Tooltip ("Does this event stay after the player has finished?")]
    public bool IsPersistent;
    [Tooltip ("Does this event play the moment the player enters the location?")]
    public bool IsAutoplay;
    [Space]
    public SOEvent[] EventsAdd;
    public SOEvent[] EventsRemove;
}
