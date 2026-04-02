using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotButton : Button
{
    private InventorySlotButtonInfo buttonInfo;

    public void Setup(InventorySlotButtonInfo buttonInfo) 
    {
        this.buttonInfo = buttonInfo;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        // skip if not interactable
        if (!interactable)
            return;

        // send Enter to button info
        buttonInfo.ButtonEnter();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        // skip if not interactable
        if (!interactable)
            return;

        // send Exit to button info
        buttonInfo.ButtonExit();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // skip if not interactable
        if (!interactable)
            return;

        // send interaction to button info
        buttonInfo.ButtonInteract();
    }
}
