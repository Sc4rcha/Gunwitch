using GameInfo;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuCombat : InventoryMenu
{
    [Header("Combat")]
    public GameObject ItemInfo;
    public GameObject ItemUseConfirm;
    [Space]
    public Button SectionBullets;
    public Button SectionConsums;
    public InventorySlotButtonInfo[] InventorySlotsLeft;
    public InventorySlotButtonInfo[] InventorySlotsRight;

    private InventoryItem consumableSelected;

    private ManagerCombat combat;

    public void Setup (PlayerInfo player, ManagerCombat combat)
    {
        this.combat = combat;

        // set inventory slot array
        inventorySlots = new InventorySlotButtonInfo[InventorySlotsRight.Length + InventorySlotsLeft.Length];
        for (int i = 0; i < InventorySlotsRight.Length; i++) 
        {
            inventorySlots[i * 2] = InventorySlotsLeft[i];
            inventorySlots[i * 2 + 1] = InventorySlotsRight[i];
        }

        // setup slots
        foreach (var slot in inventorySlots)
            slot.Setup(this);

        Setup(player);
    }

    public void ButtonConsumableUse (bool confirm)
    {
        // use consumable if confirm
        if (confirm)
        {
            // consumable effect
            ManagerGameElements.Instance.ItemReferences.GetItemReference(consumableSelected.Id).ItemEffect();
            // remove consumable from inventory
            player.Inventory.RemoveItem(consumableSelected.Type, consumableSelected.Id);
        }

        // deactivate item confirm popup
        ItemUseConfirm.SetActive(false);
        // clear selected consumable
        consumableSelected = null;
        // unlock consumable window
        LockSection(ItemType.CONSUMABLE, false);

        // refresh menu
        Refresh();
    }

    public override void ShowSection(ItemType section)
    {
        base.ShowSection(section);

        // hide all slots
        foreach (var slot in inventorySlots)
            slot.Show(false);

        //show selected section
        switch (section)
        {
            case ItemType.INGREDIENT:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.BULLET:
                for (int i = 0; i < player.Inventory.Bullets.Count; i++)
                {
                    // show inventory slot
                    inventorySlots[i].SetItem(player.Inventory.Bullets.ElementAt(i).Value);
                    inventorySlots[i].Show(true);

                    // set interactable only if player has enough mana
                    inventorySlots[i].SlotButton.interactable = player.Actor.CheckEnoughMana(player.Inventory.Bullets.ElementAt(i).Value.ManaCost);
                }
                break;
            case ItemType.DRUM:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.KEY:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.CONSUMABLE:
                for (int i = 0; i < player.Inventory.Consumables.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.Consumables[i]);
                    inventorySlots[i].Show(true);
                    inventorySlots[i].SlotButton.interactable = true;
                }
                break;
        }
    }

    public override void Lock(bool isLock)
    {
        // lock inventory sections
        SectionConsums.interactable = !isLock;
        SectionBullets.interactable = !isLock;

        base.Lock(isLock);
    }
    public override void LockSection(ItemType section, bool isLocked)
    {
        switch (section)
        {
            case ItemType.INGREDIENT:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.BULLET:
                SectionBullets.interactable = !isLocked;
                break;
            case ItemType.DRUM:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.KEY:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.CONSUMABLE:
                SectionConsums.interactable = !isLocked;
                break;
        }

        base.LockSection(section, isLocked);
    }

    public override void ItemSelect(InventoryItem item)
    {
        base.ItemSelect(item);

        if (item is Bullet bullet)
            ManagerPlayer.Instance.HUD.ReloadSelectBullet(bullet);

        ItemInfo.SetActive(true);
    }
    public override void ItemDelesect()
    {
        base.ItemDelesect();

        ItemInfo.SetActive(false);
    }
    public override void ItemUse(InventoryItem item)
    {
        // deselect used item.
        ItemDelesect();

        // load bullet
        if (item.Type == ItemType.BULLET)
        {
            combat.Player.Gun.LoadBullet(item as Bullet);
        }

        // consumable
        if (item.Type == ItemType.CONSUMABLE)
        {
            // activate item confirm popup
            ItemUseConfirm.SetActive(true);
            // set selected consumable
            consumableSelected = item;
            // lock consumable window
            LockSection(ItemType.CONSUMABLE, true);
        }
    }
}
