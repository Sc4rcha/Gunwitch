using UnityEngine;

[CreateAssetMenu(fileName = "Player Initial State", menuName = "Player/Initial State")]
public class SOPlayerInitialState : ScriptableObject
{
    [Header("Combat Stats")]
    public int Health;
    public int Mana;

    [Header("Stat")]
    public int Body;
    public int Magic;
    public int Dexterity;
    public int Luck;
    public int Charisma;

    [Header("Inventory")]
    public SOInventoryItem EquippedDrum;
    public SOInventoryItem[] StartingItems;
    public SOInventoryItemBullet[] StartingBullets;
    public SOCraftingRecipe[] StartingRecipes;

    public GameInfo.PlayerInfo GetPlayer() 
    {
        return new GameInfo.PlayerInfo(this);
    }
}
