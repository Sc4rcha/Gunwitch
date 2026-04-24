using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class DialogueSkipButton : Button
{
    ManagerDialogue dialogue;

    public void Setup(ManagerDialogue dialogue) 
    {
        this.dialogue = dialogue;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        dialogue.DialogueSkipStart();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        dialogue.DialogueSkipFinish();
    }
}
