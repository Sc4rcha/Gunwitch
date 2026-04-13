using UnityEngine;

public class SOEvent : ScriptableObject
{
    public string Name;
    public SOLocation EventLocation;
    public bool IsPersistent;
    [Space]
    public SOEvent[] EventsAdd;
    public SOEvent[] EventsRemove;
}
