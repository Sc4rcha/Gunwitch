using UnityEngine;

public class Map : MonoBehaviour
{
    [Header ("Map Information")]
    public string Name;

    public MapLocationButtonInfo[] Locations { get; private set; }

    public void Setup()
    {
        Locations = GetComponentsInChildren<MapLocationButtonInfo>();
    }
}
