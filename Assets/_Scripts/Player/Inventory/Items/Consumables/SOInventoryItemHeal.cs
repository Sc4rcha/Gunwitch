using UnityEngine;

[CreateAssetMenu(fileName = "Item Heal", menuName = "Inventory/Consumables/Item Heal")]
public class SOInventoryItemHeal : SOInventoryItemConsum
{
    [Space]
    public int HealthAmount;
    public int ManaAmount;

    public override void ItemEffect()
    {
        base.ItemEffect();

        ManagerGameElements.Instance.Player.Heal(HealthAmount);
        ManagerGameElements.Instance.Player.ManaRecover(ManaAmount);
    }
}
