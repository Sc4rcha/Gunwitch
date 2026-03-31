using GameInfo;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuOverworld : InventoryMenu
{
    [Header("References Overworld")]
    public InventorySlotButtonInfo[] InventorySlots;
    [Space]
    public RectTransform InventoryWindow;
    public GameObject InventoryBlocker;
    public Button InventoryButton;
    public Image InventorySectionIcon;
    [Space]
    public Button SectionIngredients;
    public Button SectionConsums;
    public Button SectionBullets;
    public Button SectionDrums;
    public Button SectionKeyItems;

    private bool isInventoryOpen;
    private Vector2 positionOpen = new Vector2(0, 0);
    private Vector2 positionClose = new Vector2(450, 0);

    private ItemType lastSelectedSection;

    public override void Setup(Inventory inventory)
    {
        base.Setup(inventory);

        // setup slots
        foreach (var slot in InventorySlots)
            slot.Setup(this);

        lastSelectedSection = ItemType.INGREDIENT;

        // Close inventory
        Open(false);
    }

    #region BUTTONS
    public void ButtonOpenClose()
    {
        Open(!isInventoryOpen);
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
        InventoryButton.interactable = !isLock;

        // lock inventory slots
        foreach (var slot in InventorySlots)
            slot.SlotButton.interactable = !isLock;

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
        ItemInfo.gameObject.SetActive(false);

        // show last selected section when opening the inventory
        ShowSection(lastSelectedSection);
    }
    public override void ShowSection(ItemType section)
    {
        base.ShowSection(section);

        // set last selected section
        lastSelectedSection = section;

        // hide all slots
        foreach (var slot in InventorySlots)
            slot.Show(false);

        //show selected section
        switch (section)
        {
            case ItemType.INGREDIENT:
                for (int i = 0; i < inventory.Ingredients.Count; i++)
                {
                    InventorySlots[i].SetItem(inventory.Ingredients.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.BULLET:
                for (int i = 0; i < inventory.Bullets.Count; i++)
                {
                    InventorySlots[i].SetItem(inventory.Bullets.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.DRUM:
                for (int i = 0; i < inventory.Drums.Count; i++)
                {
                    InventorySlots[i].SetItem(inventory.Drums.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.KEY:
                for (int i = 0; i < inventory.KeyItems.Count; i++)
                {
                    InventorySlots[i].SetItem(inventory.KeyItems.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.CONSUMABLE:
                for (int i = 0; i < inventory.Consumables.Count; i++)
                {
                    InventorySlots[i].SetItem(inventory.Consumables[i]);
                    InventorySlots[i].Show(true);
                }
                break;
        }

        // show section icon
        InventorySectionIcon.gameObject.SetActive(true);
    }

    public override void SelectItem(InventoryItem item)
    {
        ItemInfo.gameObject.SetActive(true);
    }
}
