using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItemReferences : MonoBehaviour
{
    public SOInventoryItem[] AllItems;

    private Dictionary<string, SOInventoryItem> itemDictionary;

    public void Setup() 
    {
        itemDictionary = new Dictionary<string, SOInventoryItem>();

        foreach (var item in AllItems)
        {
            if (!itemDictionary.ContainsKey(item.Id))
                itemDictionary.Add(item.Id, item);
            else
                Debug.LogError("Duplicate item ID: " + item.Id);
        }
    }

    public SOInventoryItem GetItemReference(string itemId) 
    {
        // return item reference if found
        if (itemDictionary.TryGetValue(itemId, out var item))
            return item;

        Debug.LogError("Item not found: " + itemId);
        return null;
    }
}
