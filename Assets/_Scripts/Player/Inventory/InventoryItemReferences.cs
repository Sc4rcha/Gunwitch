using System.Collections.Generic;
using UnityEngine;

public class InventoryItemReferences : MonoBehaviour
{
    public SOInventoryItem[] AllItems;
    public SOCraftingRecipe[] AllRecipes;

    private Dictionary<string, SOInventoryItem> itemDictionary;
    private Dictionary<string, SOCraftingRecipe> recipesDictionary;

    public void Setup() 
    {
        // setup item dictionary
        itemDictionary = new Dictionary<string, SOInventoryItem>();
        foreach (var item in AllItems)
        {
            if (!itemDictionary.ContainsKey(item.Id))
                itemDictionary.Add(item.Id, item);
            else
                Debug.LogError("Duplicate item ID: " + item.Id);
        }


        // setup recipe dictionary
        recipesDictionary = new Dictionary<string, SOCraftingRecipe>();
        foreach (var recipe in AllRecipes)
        {
            if (!itemDictionary.ContainsKey(recipe.Id))
                recipesDictionary.Add(recipe.Id, recipe);
            else
                Debug.LogError("Duplicate item ID: " + recipe.Id);
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

    public SOCraftingRecipe GetRecipeReference(string craftingId) 
    {
        // return item reference if found
        if (recipesDictionary.TryGetValue(craftingId, out var recipe))
            return recipe;

        Debug.LogError("Item not found: " + craftingId);
        return null;
    }
}
