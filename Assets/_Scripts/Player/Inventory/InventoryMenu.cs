using GameInfo;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject ItemInfo;
    
    protected Inventory inventory;

    public virtual void Setup(Inventory inventory) 
    {
        this.inventory = inventory;


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
        // hide item preview
        ItemInfo.gameObject.SetActive(false);
    }

    public virtual void SelectItem(InventoryItem item)
    {
    }
}
