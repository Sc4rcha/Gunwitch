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

        // send Enter to button info
        buttonInfo.ButtonEnter();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        // send Exit to button info
        buttonInfo.ButtonExit();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // send interaction to button info
        buttonInfo.ButtonInteract();
    }
}
