using UnityEngine;

[CreateAssetMenu(fileName = "Item Heal", menuName = "Inventory/Item Heal")]
public class SOInventoryItemHeal : SOInventoryItemConsum
{
    [Space]
    public int HealAmount;

    public override void ItemEffect()
    {
        base.ItemEffect();

        ManagerGameElements.Instance.Player.Heal(HealAmount);
    }
}
