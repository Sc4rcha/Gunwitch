using UnityEngine;

public class QuestSelectionButtonInfo : MonoBehaviour
{
    public SOQuest QuestReference;

    private QuestSelector manager;

    public void Setup(QuestSelector manager) 
    {
        this.manager = manager;
    }

    public void ButtonInteract() 
    {
        manager.SelectQuest(QuestReference);
    }
}
