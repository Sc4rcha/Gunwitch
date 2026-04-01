using GameInfo;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    protected PlayerInfo player;

    public virtual void Setup(PlayerInfo player) 
    {
        this.player = player;
    }

    #region BUTTONS
    public void ButtonOpenIngredients() => ShowSection(ItemType.INGREDIENT);
    public void ButtonOpenConsums() => ShowSection(ItemType.CONSUMABLE);
    public void ButtonOpenBullets() => ShowSection(ItemType.BULLET);
    public void ButtonOpenDrums() => ShowSection(ItemType.DRUM);
    public void ButtonOpenKeyItems() => ShowSection(ItemType.KEY);
    #endregion
    
    /// <summary>
    /// locks entire inventory menu
    /// </summary>
    /// <param name="isLock"></param>
    public virtual void Lock(bool isLock)
    {

    }
    /// <summary>
    /// Lock section button for a given section
    /// </summary>
    /// <param name="section"></param>
    public virtual void LockSection(ItemType section, bool isLocked) 
    {

    }

    public virtual void ShowSection(ItemType section)
    {
    }

    public virtual void ItemUse(InventoryItem item) { }
    public virtual void ItemSelect(InventoryItem item) { }
    public virtual void ItemDelesect() { }
}
