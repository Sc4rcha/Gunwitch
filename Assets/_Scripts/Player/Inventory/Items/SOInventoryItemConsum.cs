using UnityEngine;
using GameInfo;

public class SOInventoryItemConsum : SOInventoryItem
{
    private void OnValidate()
    {
        // force type to Consumable
        Type = ItemType.CONSUMABLE;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();

        // remove consum from player inventory
        ManagerGameElements.Instance.Player.Info.Inventory.RemoveItem(ItemType.CONSUMABLE, Id);
    }

    public override bool IsItemUsable()
    {
        return true;
    }
}
