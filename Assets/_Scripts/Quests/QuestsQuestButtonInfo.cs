using UnityEngine;

public class QuestsQuestButtonInfo : MonoBehaviour
{
    public SOQuest QuestReference;

    private ManagerQuests manager;

    public void Setup(ManagerQuests manager) 
    {
        this.manager = manager;
    }

    public void ButtonInteract() 
    {
        manager.SelectQuest(QuestReference);
    }
}
