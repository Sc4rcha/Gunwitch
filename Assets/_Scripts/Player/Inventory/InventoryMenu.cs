using GameInfo;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [Header("References UI")]
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
    [Space]
    public InventorySlotButtonInfo[] InventorySlots;
    public GameObject ItemPreview;
    
    private bool isInventoryOpen;
    private Vector2 positionOpen = new Vector2(0, 0);
    private Vector2 positionClose = new Vector2(450, 0);

    private Inventory inventory;

    public void Setup(Inventory inventory) 
    {
        this.inventory = inventory;

        // setup UI elements
        foreach (var slot in InventorySlots)
            slot.Setup(this);

        // Close inventory
        Open(false);
    }

    #region BUTTONS
    public void ButtonOpenClose()
    {
        Open(!isInventoryOpen);
    }
    public void ButtonOpenIngredients() => ShowSection(ItemType.INGREDIENT);
    public void ButtonOpenConsums() => ShowSection(ItemType.CONSUMABLE);
    public void ButtonOpenBullets() => ShowSection(ItemType.BULLET);
    public void ButtonOpenDrums() => ShowSection(ItemType.DRUM);
    public void ButtonOpenKeyItems() => ShowSection(ItemType.KEY);
    #endregion
    
    /// <summary>
    /// locks entire inventory menu
    /// </summary>
    /// <param name="isLock"></param>
    public void Lock(bool isLock)
    {
        // lock open close button
        InventoryButton.interactable = !isLock;

        // lock inventory sections
        SectionIngredients.interactable = !isLock;
        SectionConsums.interactable = !isLock;
        SectionBullets.interactable = !isLock;
        SectionDrums.interactable = !isLock;
        SectionKeyItems.interactable = !isLock;

        // lock inventory slots
        foreach (var slot in InventorySlots)
            slot.SlotButton.interactable = !isLock;
    }
    /// <summary>
    /// Lock section button for a given section
    /// </summary>
    /// <param name="section"></param>
    public void LockSection(ItemType section) 
    {
        switch (section)
        {
            case ItemType.INGREDIENT:
                SectionIngredients.interactable = false;
                break;
            case ItemType.BULLET:
                SectionBullets.interactable = false;
                break;
            case ItemType.DRUM:
                SectionDrums.interactable = false;
                break;
            case ItemType.KEY:
                SectionKeyItems.interactable = false;
                break;
            case ItemType.CONSUMABLE:
                SectionConsums.interactable = false;
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
        ItemPreview.gameObject.SetActive(false);
        // hide section icon
        InventorySectionIcon.gameObject.SetActive(false);
        // hide section slots
        foreach (var slot in InventorySlots)
            slot.Show(false);
    }

    public void ShowSection(ItemType section)
    {
        // hide all slots
        foreach (var slot in InventorySlots)
            slot.Show(false);

        // hide item preview
        ItemPreview.gameObject.SetActive(false);
        // show section icon
        InventorySectionIcon.gameObject.SetActive(true);

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
                    InventorySlots[i].SetItem(inventory.Drums.ElementAt(i).Value);
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
    }


    public void ShowItem(InventoryItem item)
    {
        ItemPreview.gameObject.SetActive(true);
    }
}
