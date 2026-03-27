using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameInfo;
public class DialogueDecisionButton : Button
{
    private TMPro.TMP_Text buttonText;
    private DecisionOption option;

    public void Setup(DecisionOption option) 
    {
        // setup varialbes
        buttonText = GetComponentInChildren<TMPro.TMP_Text>();
        this.option = option;

        // hide button
        gameObject.SetActive(false);
    }

    /// <summary>
    /// if text = null button is hidden
    /// </summary>
    /// <param name="text"></param>
    public void SetupDecisionButton(string text) 
    {
        gameObject.SetActive(text != "");
        buttonText.text = text;
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    // mouse interact with button
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // pick decision
        ManagerGameElements.Instance.ManagerDialogue.DialogueDecisionPick(option);
    }
}
