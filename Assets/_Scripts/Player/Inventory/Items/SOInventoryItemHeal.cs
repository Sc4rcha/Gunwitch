using UnityEngine;

[CreateAssetMenu(fileName = "Item Heal", menuName = "Player/Item Heal")]
public class SOInventoryItemHeal : SOInventoryItem
{
    [Space]
    public int HealAmount;

    private void OnValidate()
    {
        // force type to Consumable
        Type = GameInfo.ItemType.CONSUMABLE;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();

        ManagerGameElements.Instance.Player.Heal(HealAmount);
    }
}
