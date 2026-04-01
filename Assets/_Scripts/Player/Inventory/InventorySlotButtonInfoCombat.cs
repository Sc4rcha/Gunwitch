using UnityEngine;

public class InventorySlotButtonInfoCombat : InventorySlotButtonInfo
{
    public override void ButtonEnter()
    {
        base.ButtonEnter();

        // select item on hover
        inventoryMenu.ItemSelect(itemInfo);
    }
    public override void ButtonExit()
    {
        base.ButtonExit();

        // deselect item on hover stop
        inventoryMenu.ItemDelesect();
    }

    public override void ButtonInteract()
    {
        // use item on button interaction
        inventoryMenu.ItemUse(itemInfo);
    }
}
