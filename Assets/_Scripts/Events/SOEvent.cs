using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event/Base")]
public class SOEvent : ScriptableObject
{
    public string Name;
    public SOLocation EventLocation;
}
