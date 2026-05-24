using UnityEngine;
using GameInfo;

public class SOInventoryItemConsum : SOInventoryItem
{
    public ConsumableType ConsumableType;

    private void OnValidate()
    {
        // force type to Consumable
        Type = ItemType.CONSUMABLE;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();

        // remove consum from player inventory
        ManagerGameElements.Instance.Player.Actor.Inventory.RemoveItem(ItemType.CONSUMABLE, Id);
    }

    public override bool IsItemUsable()
    {
        switch (ConsumableType)
        {
            case ConsumableType.Hybrid:
                return true;
            case ConsumableType.Combat:
                return ManagerGameElements.Instance.CombatReference != null;
            case ConsumableType.World:
                return ManagerGameElements.Instance.CombatReference == null;
        }

        return false;
    }
}
