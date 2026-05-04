using UnityEngine;

[CreateAssetMenu(fileName = "Player Initial State", menuName = "Player/Player Initial State")]
public class SOPlayerInitialState : ScriptableObject
{
    [Header("Stat")]
    public int Body;
    public int Mind;
    public int Dexterity;
    public int Luck;
    public int Charisma;

    [Header("Inventory")]
    public SOInventoryItem EquippedDrum;
    public SOInventoryItem[] StartingItems;
    public SOInventoryItemBullet[] StartingBullets;
    public SOCraftingRecipe[] StartingRecipes;

    [Header("Stats")]
    public SOCombatConfig Stats;

    public GameInfo.ActorPlayer GetPlayer() 
    {
        return new GameInfo.ActorPlayer(this);
    }
}
