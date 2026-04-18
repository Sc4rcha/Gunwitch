using UnityEngine;
using GameInfo;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [Header("References UI")]
    public Button CraftingButton;
    public Button SelectRight;
    public Button SelectLeft;
    public GameObject CraftingUI;
    public TMPro.TMP_Text NameConsumable;
    public TMPro.TMP_Text[] IngredientList;
    public Animator CraftFailMessage;

    private bool isCraftingOpen;
    private int recipeIndex;

    private Inventory inventory;
    private InventoryMenuOverworld inventoryMenu;

    public void Setup(ManagerPlayer player) 
    {
        // get reference to inventory
        inventory = player.Info.Inventory;
        inventoryMenu = player.InventoryMenu;

        // Close crafting
        Open(false);
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

        // open close inventory, 
        inventoryMenu.Open(isCraftingOpen);

        if (isCraftingOpen)
        {
            // select ingredients and lock drums, bullets and keys
            inventoryMenu.ShowSection(ItemType.INGREDIENT);
            inventoryMenu.LockSection(ItemType.DRUM, true);
            inventoryMenu.LockSection(ItemType.BULLET, true);
            inventoryMenu.LockSection(ItemType.KEY, true);
            inventoryMenu.LockItemButtons(true);

            // show selected recipe
            ShowRecipe(recipeIndex);
        }
        else
        {
            // unlock drums, bullets and keys
            inventoryMenu.LockSection(ItemType.DRUM, false);
            inventoryMenu.LockSection(ItemType.BULLET, false);
            inventoryMenu.LockSection(ItemType.KEY, false);
            inventoryMenu.LockItemButtons(false);
        }

        // lock inventory menu open close button
        inventoryMenu.LockOpenClose(isOpen);
        inventoryMenu.LockGunSection(isOpen);
    }
    public void Lock (bool isLocked)
    {
        CraftingButton.interactable = !isLocked;
    }

    public void ButtonSelectNextRight(bool isRight) 
    {
        // move selection index
        if (isRight)
            recipeIndex = (recipeIndex + 1) % inventory.Recipes.Count;
        else
            recipeIndex = (recipeIndex - 1 + inventory.Recipes.Count) % inventory.Recipes.Count;

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
        NameConsumable.text = inventory.Recipes[index].Consumable.Name;

        // ingredient list
        for (int i = 0; i < IngredientList.Length; i++)
        {
            IngredientList[i].gameObject.SetActive(false);
            if (inventory.Recipes[index].Ingredients.Length > i)
            {
                IngredientList[i].gameObject.SetActive(true);
                IngredientList[i].text = inventory.Recipes[index].Ingredients[i].Name;
            }
        }

        // Buttons
        SelectRight.interactable = inventory.Recipes.Count > 1;
        SelectLeft.interactable = inventory.Recipes.Count > 1;
    }

    public void Craft() 
    {
        // crafting is not possible
        if (!IsCraftingPossible())
        {
            CraftFailMessage.Play("Show");
            return;
        }

        // remove ingredients from inventory
        foreach (var ingredient in inventory.Recipes[recipeIndex].IngredientsStacked)
        {
            for (int i = 0; i < ingredient.Quantity; i++)
                inventory.RemoveItem(ingredient.Type, ingredient.Id);
        }

        // add consumable to player inventory
        inventory.AddItem(inventory.Recipes[recipeIndex].Consumable);
        // refresh inventory
        inventoryMenu.Refresh();
    }
    private bool IsCraftingPossible() 
    {
        // check if player has enough ingredients
        foreach (var ingredient in inventory.Recipes[recipeIndex].IngredientsStacked)
        {
            if (!inventory.CheckIngredientInInventory(ingredient.Id, ingredient.Quantity))
                return false;
        }

        return true;
    }
}
