using UnityEngine;

[CreateAssetMenu(fileName = "Event List", menuName = "Event/Event List", order = 2)]
public class SOEventList : ScriptableObject
{
    public SOEvent[] Events;
}
