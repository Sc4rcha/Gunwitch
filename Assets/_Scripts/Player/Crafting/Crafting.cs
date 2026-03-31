using System.Collections.Generic;
using UnityEngine;
using GameInfo;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public SOCraftingRecipe[] DebugKnownRecipes;

    [Header("References UI")]
    public Button CraftingButton;
    public GameObject CraftingUI;
    public TMPro.TMP_Text NameRecipe;
    public TMPro.TMP_Text NameBullet;
    public TMPro.TMP_Text[] IngredientList;

    public List<CraftingRecipe> KnownRecipes;

    private bool isCraftingOpen;
    private int recipeIndex;

    private Inventory inventory;
    private InventoryMenuOverworld inventoryMenu;

    public void Setup(ManagerPlayer player) 
    {
        // get reference to inventory
        inventory = player.Info.Inventory;
        inventoryMenu = player.InventoryMenu;

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
    public void Open(bool isOpen)
    {
        isCraftingOpen = isOpen;

        // show UI for crafting
        CraftingUI.SetActive(isCraftingOpen);

        // open close inventory, 
        inventoryMenu.Open(isCraftingOpen);

        if (isCraftingOpen)
        {
            // select ingredients and lock drums, bullets and keys
            inventoryMenu.ShowSection(ItemType.INGREDIENT);
            inventoryMenu.LockSection(ItemType.DRUM, true);
            inventoryMenu.LockSection(ItemType.BULLET, true);
            inventoryMenu.LockSection(ItemType.KEY, true);

            // show selected recipe
            ShowRecipe(recipeIndex);
        }
        else
        {
            // unlock drums, bullets and keys
            inventoryMenu.LockSection(ItemType.DRUM, false);
            inventoryMenu.LockSection(ItemType.BULLET, false);
            inventoryMenu.LockSection(ItemType.KEY, false);
        }

    }
    public void Lock (bool isLocked)
    {
        CraftingButton.interactable = !isLocked;
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
        NameBullet.text = KnownRecipes[index].Consumable.Name;

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

        // show consumable section
        inventoryMenu.ShowSection(ItemType.CONSUMABLE);
        // add consumable to player inventory
        inventory.AddItem(KnownRecipes[recipeIndex].Consumable);

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
