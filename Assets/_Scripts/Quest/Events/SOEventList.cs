using UnityEngine;

[CreateAssetMenu(fileName = "Event List", menuName = "Event/Event List", order = 2)]
public class SOEventList : ScriptableObject
{
    public SOEvent[] Events;

    public SOEvent GetEventReference(string eventId) 
    {
        foreach (var eventReference in Events)
        {
            if (eventReference.Id == eventId)
                return eventReference;
        }

        Debug.LogError("Could not find Event with given Identifier");

        return null;
    }
}
