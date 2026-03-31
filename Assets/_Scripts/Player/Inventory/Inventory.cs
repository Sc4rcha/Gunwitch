using System.Collections.Generic;
using GameInfo;
using UnityEngine;

public class Inventory
{
    public const int MaxConsums = 6;
    public Dictionary<string, InventoryItem> Ingredients;
    public Dictionary<string, Bullet> Bullets;
    public Dictionary<string, InventoryItem> Drums;
    public Dictionary<string, InventoryItem> KeyItems;
    public List<InventoryItem> Consumables;

    public Inventory()
    {
        // setup dictionaries and lists
        Ingredients = new Dictionary<string, InventoryItem>();
        Bullets = new Dictionary<string, Bullet>();
        Drums = new Dictionary<string, InventoryItem>();
        KeyItems = new Dictionary<string, InventoryItem>();
        Consumables = new List<InventoryItem>();
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
                if (!Bullets.ContainsKey(item.Id))
                    Bullets.Add(item.Id, item as Bullet);
                else
                    Debug.LogError("There can only be one bullet of each!");
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
            case ItemType.CONSUMABLE:
                if (Consumables.Count < MaxConsums)
                    Consumables.Add(item);
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
                if (Bullets.ContainsKey(itemId))
                    Bullets.Remove(itemId);
                break;
            case ItemType.DRUM:
                if (Drums.ContainsKey(itemId))
                    Drums.Remove(itemId);
                break;
            case ItemType.KEY:
                if (KeyItems.ContainsKey(itemId))
                    KeyItems.Remove(itemId);
                break;
            case ItemType.CONSUMABLE:
                int consumIndex = Consumables.FindIndex(x => x.Id == itemId);
                if (consumIndex >= 0)
                    Consumables.RemoveAt(consumIndex);
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
                if (Bullets.ContainsKey(itemId))
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
            case ItemType.CONSUMABLE:
                if (Consumables.Find(x => x.Id == itemId) != null)
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
}
