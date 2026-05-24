using UnityEngine;

[CreateAssetMenu(fileName = "Item Save", menuName = "Inventory/Consumables/Item Save")]
public class SOInventoryItemSave : SOInventoryItemConsum
{

    public override void ItemEffect()
    {
        base.ItemEffect();

        ManagerGameElements.Instance.SaveLoad.SaveGame(true);
    }

}
