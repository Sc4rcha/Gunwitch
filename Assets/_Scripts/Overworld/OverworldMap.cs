using UnityEngine;

public class OverworldMap : MonoBehaviour
{
    [Header ("Map Information")]
    public string Name;
    public SOEventList MapEvents;

    [Header ("Scene References")]
    public OverworldLocationButtonInfo[] Locations;
}
