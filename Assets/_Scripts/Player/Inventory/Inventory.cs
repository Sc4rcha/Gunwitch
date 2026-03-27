using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameInfo;

public class Inventory : MonoBehaviour
{
    public SOInventoryItem[] DebugAddItems;

    public int MaxBullets;
    public int MaxConsums;

    [Header("References UI")]
    public RectTransform InventoryWindow;
    public GameObject InventoryBlocker;
    public Button InventoryButton;
    public Image InventorySectionIcon;
    public InventorySlotButtonInfo[] InventorySlots;
    [Header("References ItemInfo")]
    public GameObject ItemPreview;

    public Dictionary<string, InventoryItem> Ingredients;
    public List<InventoryItem> Bullets;
    public List<InventoryItem> Consumables;
    public Dictionary<string, InventoryItem> Drums;
    public Dictionary<string, InventoryItem> KeyItems;

    private bool isInventoryOpen;
    private Vector2 positionOpen = new Vector2(0, 0);
    private Vector2 positionClose = new Vector2(450, 0);

    public void Setup()
    {
        // setup dictionaries and lists
        Ingredients = new Dictionary<string, InventoryItem>();
        Drums = new Dictionary<string, InventoryItem>();
        KeyItems = new Dictionary<string, InventoryItem>();
        Bullets = new List<InventoryItem>();
        Consumables = new List<InventoryItem>();

        // setup UI elements
        foreach (var slot in InventorySlots)
            slot.Setup(this);

        // Close inventory
        Open(false);

        // add debug items
        foreach (var item in DebugAddItems)
            AddItem(item.GetItem());
    }

    public void AddItem(InventoryItem item)
    {
        switch (item.Type)
        {
            case ItemType.INGREDIENT:
                if (Ingredients.ContainsKey(item.Id))
                    Ingredients[item.Id].Quantity++;
                else
                    Ingredients.Add(item.Id, item);
                break;
            case ItemType.BULLET:
                if (Bullets.Count < MaxBullets)
                    Bullets.Add(item);
                break;
            case ItemType.CONSUMABLE:
                if (Consumables.Count < MaxConsums)
                    Consumables.Add(item);
                break;
            case ItemType.DRUM:
                if (!Drums.ContainsKey(item.Id))
                    Drums.Add(item.Id, item);
                else
                    Debug.LogError("There can only be one drum of each!");
                break;
            case ItemType.KEY:
                if (!KeyItems.ContainsKey(item.Id))
                    KeyItems.Add(item.Id, item);
                else
                    Debug.LogError("There can only be one key item of each!");
                break;
        }
    }
    public void RemoveItem(ItemType itemType, string itemId)
    {
        switch (itemType)
        {
            case ItemType.INGREDIENT:
                if (Ingredients.ContainsKey(itemId))
                {
                    Ingredients[itemId].Quantity--;
                    if (Ingredients[itemId].Quantity == 0)
                        Ingredients.Remove(itemId);
                }
                break;
            case ItemType.BULLET:
                int bulletIndex = Bullets.FindIndex(x => x.Id == itemId);
                if (bulletIndex >= 0)
                    Bullets.RemoveAt(bulletIndex);
                break;
            case ItemType.CONSUMABLE:
                int consumIndex = Consumables.FindIndex(x => x.Id == itemId);
                if (consumIndex >= 0)
                    Consumables.RemoveAt(consumIndex);
                break;
            case ItemType.DRUM:
                if (Drums.ContainsKey(itemId))
                    Drums.Remove(itemId);
                break;
            case ItemType.KEY:
                if (KeyItems.ContainsKey(itemId))
                    KeyItems.Remove(itemId);
                break;
        }
    }

    public bool CheckItemInInventory(ItemType itemType, string itemId)
    {
        switch (itemType)
        {
            case ItemType.INGREDIENT:
                if (Ingredients.ContainsKey(itemId))
                    return true;
                break;
            case ItemType.BULLET:
                if (Bullets.Find(x => x.Id == itemId) != null)
                    return true;
                break;
            case ItemType.CONSUMABLE:
                if (Consumables.Find(x => x.Id == itemId) != null)
                    return true;
                break;
            case ItemType.DRUM:
                if (Drums.ContainsKey(itemId))
                    return true;
                break;
            case ItemType.KEY:
                if (KeyItems.ContainsKey(itemId))
                    return true;
                break;
        }

        return false;
    }
    public bool CheckIngredientInInventory(string itemId, int quantity)
    {
        if (Ingredients.ContainsKey(itemId))
        {
            return Ingredients[itemId].Quantity >= quantity;
        }

        return false;
    }


    #region UI
    public void ButtonOpenClose()
    {
        Open(!isInventoryOpen);
    }
    public void ButtonSectionSelect(int itemType) 
    {
        ShowSection((ItemType)itemType);
    }
    public void Lock(bool isLock) 
    {
        InventoryButton.enabled = !isLock;
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
    public void ShowSection(ItemType type) 
    {
        // hide all slots
        foreach (var slot in InventorySlots)
            slot.Show(false);

        // hide item preview
        ItemPreview.gameObject.SetActive(false);
        // show section icon
        InventorySectionIcon.gameObject.SetActive(true);

        //show selected section
        switch (type)
        {
            case ItemType.INGREDIENT:
                for (int i = 0; i < Ingredients.Count; i++)
                {
                    InventorySlots[i].SetItem(Ingredients.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.BULLET:
                for (int i = 0; i < Bullets.Count; i++)
                {
                    InventorySlots[i].SetItem(Bullets[i]);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.CONSUMABLE:
                for (int i = 0; i < Consumables.Count; i++)
                {
                    InventorySlots[i].SetItem(Consumables[i]);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.DRUM:
                for (int i = 0; i < Drums.Count; i++)
                {
                    InventorySlots[i].SetItem(Drums.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
            case ItemType.KEY:
                for (int i = 0; i < KeyItems.Count; i++)
                {
                    InventorySlots[i].SetItem(KeyItems.ElementAt(i).Value);
                    InventorySlots[i].Show(true);
                }
                break;
        }
    }
    public void ShowItem(InventoryItem item) 
    {
        ItemPreview.gameObject.SetActive(true);
    }
    #endregion
}
