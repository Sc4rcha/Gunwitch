using GameInfo;
using UnityEngine;

public class InventoryItemInfoOverworld : InventoryItemInfo
{
    public TMPro.TMP_Text ItemName;
    [Space]
    public GameObject ConsumButton;
    public GameObject ConsumConfirm;

    [HideInInspector]
    public InventoryMenuOverworld InventoryMenu;

    private SOInventoryItem item;

    public override void InfoShow(SOInventoryItem item)
    {
        base.InfoShow(item);

        this.item = item;

        ItemName.text = item.Name;

        // show use consum if item is consum
        ConsumButton.SetActive(item.Type == ItemType.CONSUMABLE);
    }
    public void ConsumUse()
    {
        ConsumConfirm.SetActive(true);
    }

    public void ConsumUseConfirm(bool isConfirm)
    {
        ConsumConfirm.SetActive(false);

        if (isConfirm)
        {
            InventoryMenu.ConsumUse(item);
            item = null;
        }

    }
}
