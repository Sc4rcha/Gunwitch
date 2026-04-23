using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/Craft Recipe")]
public class SOCraftingRecipe : ScriptableObject
{
    public string Id;
    public string Name;

    public SOInventoryItem[] Ingredients;
    public SOInventoryItem Consumable;

    public GameInfo.CraftingRecipe GetRecipe() 
    {
        return new GameInfo.CraftingRecipe(this);
    }
}
