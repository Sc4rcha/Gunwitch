using GameInfo;
using UnityEngine;

public class InventoryItemInfoOverworld : InventoryItemInfo
{
    public TMPro.TMP_Text ItemName;
    [Space]
    public GameObject ItemUseButton;
    public GameObject ItemUseConfirmHolder;

    [HideInInspector]
    public InventoryMenuOverworld InventoryMenu;

    private SOInventoryItem item;

    public override void InfoShow(SOInventoryItem item)
    {
        base.InfoShow(item);

        this.item = item;

        ItemName.text = item.Name;

        // show use consum if item is consum
        ItemUseButton.SetActive(item.IsItemUsable());
    }
    
    public void ItemUse()
    {
        ItemUseConfirmHolder.SetActive(true);
    }
    public void ItemUseConfirm(bool isConfirm)
    {
        ItemUseConfirmHolder.SetActive(false);

        if (isConfirm)
        {
            InventoryMenu.ItemUse(item.GetItem());
            item = null;
        }
    }
}
