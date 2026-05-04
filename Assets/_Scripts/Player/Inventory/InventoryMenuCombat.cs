using GameInfo;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuCombat : InventoryMenu
{
    [Header("Combat")]
    public InventoryItemInfo Information;
    [Space]
    public GameObject ItemUseConfirm;
    public GameObject InventoryScrollview;
    [Space]
    public Button ButtonOpen;
    public Button ButtonClose;
    public TMPro.TMP_Text SectionName;
    public InventorySlotButtonInfo[] InventorySlotsLeft;
    public InventorySlotButtonInfo[] InventorySlotsRight;

    private InventoryItem consumableSelected;

    private ManagerCombat combat;

    public void Setup (ActorPlayer player, ManagerCombat combat)
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
                ButtonOpen.interactable = false;
                ButtonClose.interactable = false;
                for (int i = 0; i < player.Inventory.Bullets.Count; i++)
                {
                    // show inventory slot
                    inventorySlots[i].SetItem(player.Inventory.Bullets.ElementAt(i).Value);
                    inventorySlots[i].Show(true);

                    // set interactable only if player has enough mana
                    inventorySlots[i].SlotButton.interactable = player.CheckEnoughMana(player.Inventory.Bullets.ElementAt(i).Value.ManaCost);
                }
                break;
            case ItemType.DRUM:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.KEY:
                Debug.LogError("This section is not available in Combat");
                break;
            case ItemType.CONSUMABLE:
                ButtonOpen.interactable = false;
                ButtonClose.interactable = true;
                for (int i = 0; i < player.Inventory.Consumables.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.Consumables[i]);
                    inventorySlots[i].Show(true);
                    inventorySlots[i].SlotButton.interactable = true;
                }
                break;
        }

        // show inventory Scrollview
        InventoryScrollview.SetActive(true);
    }


    #region Button actions
    public new void ButtonOpenConsums()
    {
        combat.Player.ConsumableStart();

        ShowSection(ItemType.CONSUMABLE);
    }
    public void ButtonCloseConsums() 
    {
        combat.Player.ConsumableFinish();

        InventoryClose();
    }
    public void ButtonConsumableUse(bool confirm)
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

        // player portrait return to item selection
        PlayerHUDPortrait.Instance.ConsumsDeselect();

        // refresh menu
        Refresh();
    }
    #endregion

    public override void Lock(bool isLock)
    {
        // lock inventory sections
        ButtonOpen.interactable = !isLock;
        ButtonClose.interactable = !isLock;

        base.Lock(isLock);
    }


    #region ITEM USE
    public override void ItemSelect(InventoryItem item)
    {
        base.ItemSelect(item);

        // player portait reload select bullet
        if (item.Type == ItemType.BULLET)
            PlayerHUDPortrait.Instance.ReloadFocus(item.Id);
        // player portait reload select bullet
        if (item.Type == ItemType.CONSUMABLE)
            PlayerHUDPortrait.Instance.ConsumsFocus(item.Id);


        Information.InfoShow(ManagerGameElements.Instance.ItemReferences.GetItemReference(item.Id));
    }
    public override void ItemDelesect()
    {
        base.ItemDelesect();

        Information.InfoHide();
    }
    public override void ItemUse(InventoryItem item)
    {
        // load bullet
        if (item.Type == ItemType.BULLET)
            combat.Player.Gun.LoadBullet(item as Bullet);

        // consumable
        if (item.Type == ItemType.CONSUMABLE)
        {
            // activate item confirm popup
            ItemUseConfirm.SetActive(true);
            // set selected consumable
            consumableSelected = item;

            // player portrait take item
            PlayerHUDPortrait.Instance.ConsumsSelect(item.Id);
        }
    }
    #endregion

    public void ReloadStart() 
    {
        SectionName.text = "Bullets";
        ShowSection(ItemType.BULLET);
    }
    public void ReloadFinish() 
    {
        SectionName.text = "Consums";
        InventoryClose();
    }

    public void InventoryClose() 
    {
        ItemDelesect();

        ButtonClose.interactable = false;
        ButtonOpen.interactable = true;

        // hide all slots
        foreach (var slot in inventorySlots)
            slot.Show(false);

        // hide inventory Scrollview
        InventoryScrollview.SetActive(false);
    }
}
