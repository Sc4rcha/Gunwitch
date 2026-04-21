using UnityEngine;

public class ManagerQuests : MonoBehaviour
{
    public GameObject QuestAvailable;
    public GameObject QuestConfirm;
    public QuestsQuestButtonInfo[] Buttons;
    [Space]
    public TMPro.TMP_Text QuestDescription;

    private SOQuest quest;

    public void Setup() 
    {
        foreach (var button in Buttons)
            button.Setup(this);
    }

    public void Show(bool isShow) 
    {
        gameObject.SetActive(isShow);
        
        if (isShow)
            QuestAvailable.SetActive(true);

        QuestConfirm.SetActive(false);
    }

    /// <summary>
    /// called by button to select a quest and show confirm menu
    /// </summary>
    /// <param name="quest"></param>
    public void SelectQuest(SOQuest quest) 
    {
        QuestAvailable.SetActive(false);
        QuestConfirm.SetActive(true);

        QuestDescription.text = quest.QuestDescription;

        this.quest = quest;
    }


    public void ButtonConfirm(bool isConfirm) 
    {
        QuestConfirm.SetActive(true);

        if (isConfirm)
            ManagerGameElements.Instance.QuestStart(quest);
        else
        {
            QuestAvailable.SetActive(true);
            QuestConfirm.SetActive(false);
            quest = null;
        }
    }
}
