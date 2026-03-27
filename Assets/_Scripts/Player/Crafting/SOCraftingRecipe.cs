using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Player/Recipe")]
public class SOCraftingRecipe : ScriptableObject
{
    public string Id;
    public string Name;

    public SOInventoryItem[] Ingredients;
    public SOInventoryItem Bullet;

    public GameInfo.CraftingRecipe GetRecipe() 
    {
        return new GameInfo.CraftingRecipe(this);
    }
}
