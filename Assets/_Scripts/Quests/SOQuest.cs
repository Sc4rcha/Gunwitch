using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Map/Quest")]
public class SOQuest : ScriptableObject
{
    public string QuestName;
    [TextArea]
    public string QuestDescription;
    [Space]
    public SOEvent InitialEvent;
    public Map QuestMap;
}
