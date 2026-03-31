using UnityEngine;

public class Map : MonoBehaviour
{
    [Header ("Map Information")]
    public string Name;
    public SOEventList MapEvents;

    [Header ("Scene References")]
    public MapLocationButtonInfo[] Locations;
}
