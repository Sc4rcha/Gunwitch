using System.Collections.Generic;
using UnityEngine;
using GameInfo;

public class Crafting : MonoBehaviour
{
    public SOCraftingRecipe[] DebugKnownRecipes;

    [Header("References UI")]
    public GameObject CraftingUI;
    public TMPro.TMP_Text NameRecipe;
    public TMPro.TMP_Text NameBullet;
    public TMPro.TMP_Text[] IngredientList;

    public List<CraftingRecipe> KnownRecipes;

    private bool isCraftingOpen;
    private int recipeIndex;

    private Inventory inventory;

    public void Setup() 
    {
        // get reference to inventory
        inventory = ManagerGameElements.Instance.Inventory;

        // setup recipes known list
        KnownRecipes = new List<CraftingRecipe>();

        // Close crafting
        Open(false);

        // add debug known recipes
        foreach (var recipe in DebugKnownRecipes)
            AddRecipe(recipe.GetRecipe());
    }

    public void AddRecipe(CraftingRecipe recipe) 
    {
        KnownRecipes.Add(recipe);
    }
    public void RemoveRecipe(CraftingRecipe recipe)
    {
        int index = KnownRecipes.FindIndex(x => x.Id == recipe.Id);
        if (index >= 0)
            KnownRecipes.RemoveAt(index);
    }


    public void ButtonOpenClose() 
    {
        Open(!isCraftingOpen);
    }
    public void Open (bool isOpen)
    {
        isCraftingOpen = isOpen;

        // show UI for crafting
        CraftingUI.SetActive(isCraftingOpen);

        // stop if closing crafting menu
        if (!isCraftingOpen)
            return;

        // open inventory and select ingredients
        inventory.Open(true);
        inventory.ShowSection(ItemType.INGREDIENT);

        // show selected recipe
        ShowRecipe(recipeIndex);
    }

    public void ButtonSelectNextRight(bool isRight) 
    {
        // move selection index
        if (isRight)
            recipeIndex = (recipeIndex + 1) % KnownRecipes.Count;
        else
            recipeIndex = (recipeIndex - 1 + KnownRecipes.Count) % KnownRecipes.Count;

        // show recipe
        ShowRecipe(recipeIndex);
    }
    public void ButtonCraft() 
    {
        Craft();
    }

    private void ShowRecipe(int index) 
    {
        // Recipes names
        NameRecipe.text = KnownRecipes[index].Name;
        NameBullet.text = KnownRecipes[index].Bullet.Name;

        // ingredient list
        for (int i = 0; i < IngredientList.Length; i++)
        {
            IngredientList[i].gameObject.SetActive(false);
            if (KnownRecipes[index].Ingredients.Length > i)
            {
                IngredientList[i].gameObject.SetActive(true);
                IngredientList[i].text = KnownRecipes[index].Ingredients[i].Name;
            }
        }
    }

    public void Craft() 
    {
        // crafting is not possible
        if (!IsCraftingPossible())
            return;

        // remove ingredients from inventory
        foreach (var ingredient in KnownRecipes[recipeIndex].IngredientsStacked)
        {
            for (int i = 0; i < ingredient.Quantity; i++)
                inventory.RemoveItem(ingredient.Type, ingredient.Id);
        }

        // add bullet to player inventory
        inventory.AddItem(KnownRecipes[recipeIndex].Bullet);
        // refresh inventory
        inventory.ShowSection(ItemType.INGREDIENT);
    }
    private bool IsCraftingPossible() 
    {
        // check if player has enough ingredients
        foreach (var ingredient in KnownRecipes[recipeIndex].IngredientsStacked)
        {
            if (!inventory.CheckIngredientInInventory(ingredient.Id, ingredient.Quantity))
                return false;
        }

        return true;
    }
}
