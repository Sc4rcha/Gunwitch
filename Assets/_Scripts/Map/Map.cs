using UnityEngine;

public class Map : MonoBehaviour
{
    [Header ("Map Information")]
    public string Name;
    public SOEventList MapEvents;
    public SOLocation InitialLocation;

    [Header ("Scene References")]
    public MapLocationButtonInfo[] Locations;
}
