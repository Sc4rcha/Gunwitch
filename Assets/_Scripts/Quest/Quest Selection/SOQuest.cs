using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Map/Quest")]
public class SOQuest : ScriptableObject
{
    public string QuestName;
    [TextArea]
    public string QuestDescription;
    [Space]
    public SOLocation InitialLocation;
    public SOLocationList AllLocations;
    [Space]
    public SOEvent InitialEvent;
    public SOEventList ActiveEvents;
    public SOEventList EventsAll;

    public Map QuestMap;
}
