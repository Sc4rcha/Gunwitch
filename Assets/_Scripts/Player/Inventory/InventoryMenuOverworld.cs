using GameInfo;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuOverworld : InventoryMenu
{
    [Header("References Overworld")]
    public InventoryItemInfoOverworld Information;
    public InventoryGun Gun;
    [Space]
    public InventorySlotButtonInfo[] InventorySlotsReference;
    [Space]
    public RectTransform InventoryWindow;
    public GameObject InventoryBlocker;
    public Button InventoryButton;
    public Image InventorySectionIcon;
    [Space]
    public TMPro.TMP_Text SectionName;
    public Button SectionIngredients;
    public Button SectionConsums;
    public Button SectionBullets;
    public Button SectionDrums;
    public Button SectionKeyItems;
    public Button SectionGun;
    [Header("References Project")]
    public Sprite IconIngredient;
    public Sprite IconConsumable;
    public Sprite IconBullet;
    public Sprite IconKey;
    public Sprite IconDrum;

    private bool isInventoryOpen;
    private Vector2 positionOpen = new Vector2(0, 0);
    private Vector2 positionClose = new Vector2(450, 0);

    private ItemType lastSelectedSection;

    public override void Setup(PlayerInfo player)
    {
        base.Setup(player);

        // setup slots
        inventorySlots = InventorySlotsReference;
        foreach (var slot in inventorySlots)
            slot.Setup(this);

        // select ingredients by default
        lastSelectedSection = ItemType.INGREDIENT;

        // setup information
        Information.InventoryMenu = this;

        // setup gun
        Gun.Setup(player);

        // Close inventory
        Open(false);
    }

    #region BUTTONS
    public void ButtonOpenClose()
    {
        Open(!isInventoryOpen);
    }
    public void ButtonGun() 
    {
        Gun.OpenClose();
    }
    #endregion

    /// <summary>
    /// locks entire inventory menu
    /// </summary>
    /// <param name="isLock"></param>
    public override void Lock(bool isLock)
    {
        // lock inventory sections
        SectionIngredients.interactable = !isLock;
        SectionConsums.interactable = !isLock;
        SectionBullets.interactable = !isLock;
        SectionDrums.interactable = !isLock;
        SectionKeyItems.interactable = !isLock;

        // lock open close button
        LockOpenClose(isLock);

        base.Lock(isLock);
    }
    public override void LockSection(ItemType section, bool isLocked)
    {
        switch (section)
        {
            case ItemType.INGREDIENT:
                SectionIngredients.interactable = !isLocked;
                break;
            case ItemType.BULLET:
                SectionBullets.interactable = !isLocked;
                break;
            case ItemType.DRUM:
                SectionDrums.interactable = !isLocked;
                break;
            case ItemType.KEY:
                SectionKeyItems.interactable = !isLocked;
                break;
            case ItemType.CONSUMABLE:
                SectionConsums.interactable = !isLocked;
                break;
        }

        base.LockSection(section, isLocked);
    }
    public void LockOpenClose(bool isLock) 
    {
        // lock open close button
        InventoryButton.interactable = !isLock;
    }
    public void LockGunSection (bool isLock) 
    {
        SectionGun.interactable = !isLock;
    }

    public void Open(bool isOpen)
    {
        isInventoryOpen = isOpen;

        // activate / deactivate raycast blocker
        InventoryBlocker.SetActive(isOpen);

        // move inventory panel
        if (isOpen)
            InventoryWindow.anchoredPosition = positionOpen;
        else
            InventoryWindow.anchoredPosition = positionClose;

        // hide item preview
        Information.InfoHide();
        // hide gun
        Gun.Hide();

        // show last selected section when opening the inventory
        ShowSection(lastSelectedSection);
    }
    public override void ShowSection(ItemType section)
    {
        base.ShowSection(section);

        // hide item info if changing sections
        Information.InfoHide();

        // set last selected section
        lastSelectedSection = section;

        // hide all slots
        foreach (var slot in inventorySlots)
            slot.Show(false);

        //show selected section
        switch (section)
        {
            case ItemType.INGREDIENT:
                for (int i = 0; i < player.Inventory.Ingredients.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.Ingredients.ElementAt(i).Value);
                    inventorySlots[i].Show(true);
                }

                InventorySectionIcon.sprite = IconIngredient;
                SectionName.text = "Ingredients";

                break;
            case ItemType.BULLET:
                for (int i = 0; i < player.Inventory.Bullets.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.Bullets.ElementAt(i).Value);
                    inventorySlots[i].Show(true);
                }

                InventorySectionIcon.sprite = IconBullet;
                SectionName.text = "Bullets";

                break;
            case ItemType.DRUM:
                for (int i = 0; i < player.Inventory.Drums.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.Drums.ElementAt(i).Value);
                    inventorySlots[i].Show(true);
                }

                InventorySectionIcon.sprite = IconDrum;
                SectionName.text = "Drums";

                break;
            case ItemType.KEY:
                for (int i = 0; i < player.Inventory.KeyItems.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.KeyItems.ElementAt(i).Value);
                    inventorySlots[i].Show(true);
                }

                InventorySectionIcon.sprite = IconKey;
                SectionName.text = "Key items";

                break;
            case ItemType.CONSUMABLE:
                for (int i = 0; i < player.Inventory.Consumables.Count; i++)
                {
                    inventorySlots[i].SetItem(player.Inventory.Consumables[i]);
                    inventorySlots[i].Show(true);
                }

                InventorySectionIcon.sprite = IconConsumable;
                SectionName.text = "Consumables";

                break;
        }

        // show section icon
        InventorySectionIcon.gameObject.SetActive(true);
    }

    public override void ItemUse(InventoryItem item)
    {
        base.ItemUse(item);

        // get scriptable object of item
        SOInventoryItem soItem = ManagerGameElements.Instance.ItemReferences.GetItemReference(item.Id);

        // item effect
        soItem.ItemEffect();

        // Refresh inventory
        Refresh();
    }
    public override void ItemSelect(InventoryItem item)
    {

        // show item info
        Information.InfoShow(ManagerGameElements.Instance.ItemReferences.GetItemReference(item.Id));
    }
}
