using GameInfo;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuCombat : InventoryMenu
{
    [Header("Combat")]
    public Button SectionConsums;
    public Button SectionBullets;
    public InventorySlotButtonInfo[] InventorySlotsLeft;
    public InventorySlotButtonInfo[] InventorySlotsRight;

    private InventorySlotButtonInfo[] inventorySlots;

    private ManagerCombat combat;

    public void Setup (Inventory inventory, ManagerCombat combat)
    {
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

        Setup(inventory);

        this.combat = combat;
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
                for (int i = 0; i < inventory.Bullets.Count; i++)
                {
                    inventorySlots[i].SetItem(inventory.Bullets.ElementAt(i).Value);
                    inventorySlots[i].Show(true);
                }
                break;
            case ItemType.DRUM:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.KEY:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.CONSUMABLE:
                for (int i = 0; i < inventory.Consumables.Count; i++)
                {
                    inventorySlots[i].SetItem(inventory.Consumables[i]);
                    inventorySlots[i].Show(true);
                }
                break;
        }
    }

    public override void Lock(bool isLock)
    {
        // lock inventory sections
        SectionConsums.interactable = !isLock;
        SectionBullets.interactable = !isLock;

        // lock inventory slots
        foreach (var slot in inventorySlots)
            slot.SlotButton.interactable = !isLock;

        base.Lock(isLock);
    }
    public override void LockSection(ItemType section)
    {
        switch (section)
        {
            case ItemType.INGREDIENT:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.BULLET:
                SectionBullets.interactable = false;
                break;
            case ItemType.DRUM:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.KEY:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.CONSUMABLE:
                SectionConsums.interactable = false;
                break;
        }
    }

    public override void SelectItem(InventoryItem item)
    {
        if (item is Bullet bullet)
            combat.Player.Gun.LoadBullet(bullet);
    }
}
